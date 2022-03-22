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
using VietCapital.Partner.F5Seconds.WebApp.HostedService;
using VietCapital.Partner.F5Seconds.WebApp.Repositories;

namespace VietCapital.Partner.F5Seconds.WebApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddHostedService(this IServiceCollection services)
        {
            services.AddHostedService<RedisCacheHostedService>();
        }
        public static void AddRedisCacheExtension(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "VietcapitalInstance";
            });
        }
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
            services.AddHttpClient<IGatewayHttpClientService, GatewayHttpClientRepository>(c =>
            {
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
                x.AddConsumer<GatewayProductConsumer>();
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
        public static void AddAuthorizations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("adminPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && claim.Value == "admin")
                                 )
                );
                options.AddPolicy("userSeenPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/danh-sach-user;seen" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("userCreatePolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/danh-sach-user;create" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("userEditPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/danh-sach-user;edit" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("userDeletePolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/danh-sach-user;delete" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("sanPhamSeenPolicy",
                 policy => policy.RequireAssertion(
                      context => context.User.HasClaim(claim =>
                                      claim.Type == "quyen" && (claim.Value == "/san-pham;seen" || claim.Value == "admin")
                                  ))
                 );
                options.AddPolicy("sanPhamEditPolicy",
                 policy => policy.RequireAssertion(
                      context => context.User.HasClaim(claim =>
                                      claim.Type == "quyen" && (claim.Value == "/san-pham;edit" || claim.Value == "admin")
                                  ))
                 );
                options.AddPolicy("donHangSeenPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/don-hang/danh-sach-don-hang;seen" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("phanQuyenDeletePolicy",
                policy => policy.RequireAssertion(
                context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/phan-quyen-user;delete" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("phanQuyenEditPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/phan-quyen-user;edit" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("phanQuyenCreatePolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/phan-quyen-user;create" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("phanQuyenSeenPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/quan-ly-user/phan-quyen-user;seen" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("danhMucSeenPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/danh-muc;seen" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("danhMucCreatePolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/danh-muc;create" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("danhMucEditPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/danh-muc;edit" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("danhMucDeletePolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/danh-muc;delete" || claim.Value == "admin")
                                 ))
                );
                options.AddPolicy("doiSoatSeenPolicy",
                policy => policy.RequireAssertion(
                     context => context.User.HasClaim(claim =>
                                     claim.Type == "quyen" && (claim.Value == "/don-hang/doi-soat;seen" || claim.Value == "admin")
                                 ))
                );
            });

        }
        public static void AddRepositoryExtension(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
        }
    }
}
