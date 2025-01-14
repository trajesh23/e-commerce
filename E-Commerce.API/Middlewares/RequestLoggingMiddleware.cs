using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestUrl = context.Request.Path;
            var requestTime = DateTime.Now;

            // User Id
            var userId = context.User.Claims.FirstOrDefault(o => 
                o.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (userId == null)
            {
                userId = "Anonymous";
                _logger.LogWarning("No userId found for the request at {RequestTime}. URL: {RequestUrl}", requestTime, requestUrl);
            }

            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation("Request: {RequestUrl}, Time: {RequestTime}, User: {UserId}, Duration: {Duration}ms",
                requestUrl, requestTime, userId, stopwatch.ElapsedMilliseconds);
        }
    }
}
