using Store.Route.Core;
using Store.Route.Core.Entities;
using Store.Route.Core.Entities.Order;
using Store.Route.Core.Services.Contract;
using Store.Route.Core.Specifications.Orders;
using Store.Route.Service.Services.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IPaymentService paymentService,IBasketService basketService,IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _basketService = basketService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {

            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var orderItems = new List<OrderItem>();

            if(basket.Items.Count() > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);

                    var ProductOrderItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(ProductOrderItem,product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                } 
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var ExistOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                if (ExistOrder != null)
                {
                    _unitOfWork.Repository<Order, int>().Delete(ExistOrder);
                }
            }
        
            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems,subTotal,basketDto.PaymentIntentId);

            await _unitOfWork.Repository<Order, int>().AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;

            return order;
        }

        public async Task<Order>? GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec= new OrderSpecifications(buyerEmail);
            var orders = await  _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders is null) return null;
            return orders;
        }
    }
}
