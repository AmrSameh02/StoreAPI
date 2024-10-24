using Microsoft.Extensions.Configuration;
using Store.Route.Core;
using Store.Route.Core.DTOs.Baskets;
using Store.Route.Core.Entities;
using Store.Route.Core.Entities.Order;
using Store.Route.Core.Services.Contract;
using Store.Route.Service.Services.Baskets;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Route.Core.Entities.Product;

namespace Store.Route.Service.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            //Get Basket
            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var subTotal =  basket.Items.Sum( i => i.Price * i.Quantity);

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent; 

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(shippingPrice * 100 + subTotal * 100),
                    PaymentMethodTypes = new List<string>() {"card"},
                    Currency = "usd"
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(shippingPrice * 100 + subTotal * 100)
                };

                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            basket = await _basketService.UpdateBasketAsync(basket);

            if (basket is null) return null;

            return basket;
        }
    }
}
