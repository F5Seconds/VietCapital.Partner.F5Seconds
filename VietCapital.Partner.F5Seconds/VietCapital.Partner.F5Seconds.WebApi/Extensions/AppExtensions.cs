using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using VietCapital.Partner.F5Seconds.WebApi.Middlewares;

namespace VietCapital.Partner.F5Seconds.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VietCapital.Partner.F5Seconds.WebApi");
            });
        }
        public static void UseMiddlewareExtension(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<AdminSafeListMiddleware>(configuration["AdminSafeList"]);
        }
    }
}
