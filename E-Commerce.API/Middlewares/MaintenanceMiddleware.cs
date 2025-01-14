using E_Commerce.Business.Interfaces;

namespace E_Commerce.API.Middlewares
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var _settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenanceMode = _settingService.GetMaintenanceState();

            if (context.Request.Path.StartsWithSegments("/api/auth/login") ||
                context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context);
                return;
            }

            if (maintenanceMode)
            {
                await context.Response.WriteAsync("Currently under construction.");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
