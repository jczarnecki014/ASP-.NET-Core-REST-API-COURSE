using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopwath;
        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwath= new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwath.Start();

            await next.Invoke(context);

            _stopwath.Stop();

            var time = _stopwath.ElapsedMilliseconds/100;

            if(time > 4)
            {
                _logger.LogWarning($"{context.Request.Method} at {context.Request.Path} took {time} seccond");
            }
        }
    }
}
