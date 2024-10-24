using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Route.Core.DTOs.Auth;
using Store.Route.Core.DTOs.Orders;
using Store.Route.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Mapping.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile(IConfiguration configuration) 
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, op => op.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, op => op.MapFrom(s => s.DeliveryMethod.Cost))
                .ForMember(d => d.Total, op => op.MapFrom(s => s.GetTotal()));

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, op => op.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, op => op.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, op => op.MapFrom(s => $"{configuration["BaseUrl"]}{s.Product.PictureUrl}"));
        }
    }
}
