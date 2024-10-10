using StackExchange.Redis;
using Store.Route.Core.Entities;
using Store.Route.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Route.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string bsketId)
        {
            var basket = await _database.StringGetAsync(bsketId);
            
            return basket.IsNullOrEmpty ? null: JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (createdOrUpdatedBasket is false) return null;

            return await GetBasketAsync(basket.Id);

        }
    }
}
