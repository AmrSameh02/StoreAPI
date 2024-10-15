using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Route.Core.Services.Contract;
using System.Text;

namespace Store.Route.APIs.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CacheAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCacheKeyAsync(cacheKey);
            
            if(!string.IsNullOrEmpty(cacheResponse)) 
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if(executedContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheKeyAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_expireTime));
            }
            


        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach ( var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString();
        }
    }
}
