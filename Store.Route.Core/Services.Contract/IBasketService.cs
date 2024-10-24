using Store.Route.Core.DTOs.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Service.Services.Baskets
{
    public interface IBasketService
    {
        Task<CustomerBasketDto?> GetBasketAsync(string basketId);
        Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto basketDto);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
