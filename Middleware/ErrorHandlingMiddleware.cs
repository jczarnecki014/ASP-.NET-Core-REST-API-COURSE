using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;
using System;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException error)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Restaurant not found");
            }
            catch(Exception error)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("something went wrong");

                _logger.LogError(error, error.Message);

            }
        }
    }
}
