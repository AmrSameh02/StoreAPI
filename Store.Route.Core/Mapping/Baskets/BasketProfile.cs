using AutoMapper;
using Store.Route.Core.DTOs.Baskets;
using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Mapping.Baskets
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
        }
    }
}
