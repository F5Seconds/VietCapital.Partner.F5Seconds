using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VietCapital.Partner.F5Seconds.Application;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Identity;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared;
using VietCapital.Partner.F5Seconds.WebMvc.Extensions;
using VietCapital.Partner.F5Seconds.WebMvc.Services;

namespace VietCapital.Partner.F5Seconds.WebMvc
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Startup> _logger;
        public IConfiguration _config { get; }
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config,_env);
            services.AddPersistenceInfrastructure(_config,_env.IsProduction());
            services.AddSharedInfrastructure(_config);
            services.AddHttpClientExtension(_config,_env);
            services.AddRabbitMqExtension(_config,_env);
            services.AddHostedService();
            services.AddAuthorizeCookieExtension();
            services.AddSession();
            services.AddMvcCore().AddMetricsCore();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpClientExtension(_config,_env);
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
