using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VietCapital.Partner.F5Seconds.Application;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared;
using VietCapital.Partner.F5Seconds.WebApi.Extensions;
using VietCapital.Partner.F5Seconds.WebApi.Services;

namespace VietCapital.Partner.F5Seconds.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config,_env);
            services.AddPersistenceInfrastructure(_config, _env.IsProduction());
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddHttpClientExtension(_config,_env);
            services.AddRabbitMqExtension(_config,_env);
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}
