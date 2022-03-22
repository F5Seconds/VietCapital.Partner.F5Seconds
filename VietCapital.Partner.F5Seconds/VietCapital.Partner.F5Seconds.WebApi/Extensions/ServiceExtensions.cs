using AspNetCoreRateLimit;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;
using VietCapital.Partner.F5Seconds.WebApi.Consumer;
using VietCapital.Partner.F5Seconds.WebApi.HostedService;

namespace VietCapital.Partner.F5Seconds.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddHostedService(this IServiceCollection services)
        {
            services.AddHostedService<LoadTransactionIdToCacheHostedService>();
            services.AddHostedService<LoadProductCodeToCacheHostedService>();
        }
        public static void AddRateLimitExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.Configure<ClientRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "2s",
                        Limit = 1,
                    }
                };
            });
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }
        public static void AddRedisCacheExtension(this IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
        {
            var redisConfig = configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfig);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["ConnectionStrings:Redis"];
                options.InstanceName = "VietcapitalInstance";
            });

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
            if (env.IsProduction())
            {
                rabbitHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Host);
                rabbitvHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Vhost);
                rabbitUser = Environment.GetEnvironmentVariable(RabbitMqEnvConst.User);
                rabbitPass = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Pass);
                voucherTransactionQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.voucherTransactionQueue);
                channelUpdateStateQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.channelUpdateStateQueue);
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
                }));
            });
            services.AddMassTransitHostedService();
        }
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"VietCapital.Partner.F5Seconds.WebApi.xml"));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VietCapital.Partner.F5Seconds.WebApi",
                    Description = "This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = "Le Anh Toan",
                        Email = "toanle@f5seconds.vn",
                        Url = new Uri("https://f5seconds.vn"),
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
