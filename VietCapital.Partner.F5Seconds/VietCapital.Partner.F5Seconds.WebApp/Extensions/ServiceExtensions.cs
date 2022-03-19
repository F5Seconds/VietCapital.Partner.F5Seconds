using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;
using VietCapital.Partner.F5Seconds.WebApp.Consumer;

namespace VietCapital.Partner.F5Seconds.WebApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"VietCapital.Partner.F5Seconds.WebApp.xml"));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VietCapital.Partner.F5Seconds.WebApp",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "codewithmukesh",
                        Email = "hello@codewithmukesh.com",
                        Url = new Uri("https://codewithmukesh.com/contact"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }
        public static void AddNewtonsoftJson(this IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
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
        public static void AddRabbitMqExtension(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            string rabbitHost = configuration[RabbitMqAppSettingConst.Host];
            string rabbitvHost = configuration[RabbitMqAppSettingConst.Vhost];
            string rabbitUser = configuration[RabbitMqAppSettingConst.User];
            string rabbitPass = configuration[RabbitMqAppSettingConst.Pass];
            string voucherTransactionQueue = configuration[RabbitMqAppSettingConst.voucherTransactionQueue];
            string channelUpdateStateQueue = configuration[RabbitMqAppSettingConst.channelUpdateStateQueue];
            string productSyncQueue = configuration[RabbitMqAppSettingConst.productSyncQueue];
            if (env.IsProduction())
            {
                rabbitHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Host);
                rabbitvHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Vhost);
                rabbitUser = Environment.GetEnvironmentVariable(RabbitMqEnvConst.User);
                rabbitPass = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Pass);
                voucherTransactionQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.voucherTransactionQueue);
                channelUpdateStateQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.channelUpdateStateQueue);
                productSyncQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.productSyncQueue);
            }
            services.AddMassTransit(x =>
            {
                x.AddConsumer<VoucherTransactionConsumer>();
                x.AddConsumer<ChannelUpdateStateConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(rabbitHost, rabbitvHost, h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPass);
                    });
                    config.ReceiveEndpoint(voucherTransactionQueue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<VoucherTransactionConsumer>(provider);
                    });
                    config.ReceiveEndpoint(channelUpdateStateQueue, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<ChannelUpdateStateConsumer>(provider);
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
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }
    }
}
