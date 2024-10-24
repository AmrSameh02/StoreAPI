using Store.Route.Core.DTOs.Baskets;
using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);

    }
}
