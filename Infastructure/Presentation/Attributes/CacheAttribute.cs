using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int DurationInSec = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Create CacheKey
            string CacheKey = CreateCacheKey(context.HttpContext.Request);
            //Search for value with cache key
            ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheValue = await cacheService.GetAsync(CacheKey);
            //Return value if not null
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            //if null
            //invoke next
            var executedContext = await next.Invoke();

            //Set value(response) with cache key
            if (executedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(CacheKey, result, TimeSpan.FromSeconds(DurationInSec));
            }
            //return value

        }
        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path);
            Key.Append("?");

            foreach (var item in request.Query.OrderBy(Q => Q.Key))
            {
                Key.Append($"{item.Key}={item.Value}&");
            }
            return Key.ToString();
        }
    }
}
