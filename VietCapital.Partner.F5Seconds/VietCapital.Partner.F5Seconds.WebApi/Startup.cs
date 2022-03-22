using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<Startup> _logger;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            _logger = loggerFactory.CreateLogger<Startup>();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config,_env);
            services.AddPersistenceInfrastructure(_config, _env.IsProduction());
            services.AddSharedInfrastructure(_config);
            services.AddSwaggerExtension();
            services.AddRedisCacheExtension(_config,_logger);
            services.AddHttpClientExtension(_config,_env);
            services.AddRabbitMqExtension(_config,_env);
            //services.AddRateLimitExtension(_config);
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddHostedService();
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
            app.UseResponseCaching();
            app.UseSwaggerExtension();
            app.UseMiddlewareExtension(_config);
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}
