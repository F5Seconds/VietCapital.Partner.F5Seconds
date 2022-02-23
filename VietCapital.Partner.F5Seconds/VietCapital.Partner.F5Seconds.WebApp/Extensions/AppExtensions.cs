using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.WebApp.Middlewares;

namespace VietCapital.Partner.F5Seconds.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.VietCapital.Partner.F5Seconds.WebApi");
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
