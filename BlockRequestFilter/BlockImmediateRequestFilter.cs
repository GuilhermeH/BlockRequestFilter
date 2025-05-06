using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace BlockRequestFilter
{
    public class BlockImmediateRequestFilter : IActionFilter
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _blockDuration;

        public BlockImmediateRequestFilter(IMemoryCache cache, int seconds = 5)
        {
            _cache = cache;
            _blockDuration = TimeSpan.FromSeconds(seconds);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            var clientId = httpContext.Connection.RemoteIpAddress?.ToString();

            var endpoint = httpContext.Request.Path.ToString().ToLower();
            var cacheKey = $"request-block:{clientId}:{endpoint}";

            if (_cache.TryGetValue(cacheKey, out _))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 429,
                    Content = "Please wait a moment before trying again."
                };
                return;
            }

            _cache.Set(cacheKey, true, _blockDuration);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
