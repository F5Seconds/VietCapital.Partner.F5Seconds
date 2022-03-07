using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;
using VietCapital.Partner.F5Seconds.WebMvc.Consumer;
using VietCapital.Partner.F5Seconds.WebMvc.HostedService;

namespace VietCapital.Partner.F5Seconds.WebMvc.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddHttpClientExtension(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            string gateWayUri = configuration["Gateway:Uri"];
            if (env.IsProduction())
            {
                gateWayUri = Environment.GetEnvironmentVariable("GATEWAY_URI");
            }
            services.AddHttpClient<IGatewayHttpClientService, GatewayHttpClientRepository>(c => {
                c.BaseAddress = new Uri(gateWayUri);
            });
        }

        public static void AddAuthorizeCookieExtension(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = x =>
                    {
                        x.Response.Redirect("/Identity/Account/Login");
                        return Task.CompletedTask;
                    }
                };
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
        }
        public static void AddRabbitMqExtension(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            string rabbitHost = configuration[RabbitMqAppSettingConst.Host];
            string rabbitvHost = configuration[RabbitMqAppSettingConst.Vhost];
            string rabbitUser = configuration[RabbitMqAppSettingConst.User];
            string rabbitPass = configuration[RabbitMqAppSettingConst.Pass];
            string productSyncQueue = configuration[RabbitMqAppSettingConst.productSyncQueue];
            if (env.IsProduction())
            {
                rabbitHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Host);
                rabbitvHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Vhost);
                rabbitUser = Environment.GetEnvironmentVariable(RabbitMqEnvConst.User);
                rabbitPass = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Pass);
                productSyncQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.productSyncQueue);
            }

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GatewayProductConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(rabbitHost, rabbitvHost, h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPass);
                    });
                    config.ReceiveEndpoint(productSyncQueue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<GatewayProductConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
        }

        public static void AddHostedService(this IServiceCollection services)
        {
            services.AddHostedService<GatewayProductSyncHostedService>();
        }
    }
}
