using BookingSystem.Api.Middlewares;

namespace BookingSystem.Api.Configurations
{
    public static class MiddlewareConfiguration
    {
        public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
