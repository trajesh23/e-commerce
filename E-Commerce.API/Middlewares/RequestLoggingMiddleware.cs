using Microsoft.AspNetCore.Http;
using Serilog;

namespace E_Commerce.API.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // İstek URL'si ve zamanı
            var requestUrl = context.Request.Path;
            var requestTime = DateTime.UtcNow;

            // Kullanıcı kimliği
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            // Loglama
            Log.Information("Request: {RequestUrl}, Time: {RequestTime}, User: {UserId}",
                requestUrl, requestTime, userId);

            // Bir sonraki middleware'e geç
            await _next(context);
        }
    }


}
