using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string bsketId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);
            
    }
}
