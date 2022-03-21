using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;

namespace VietCapital.Partner.F5Seconds.WebApp.Repositories
{
    public interface IProductRepository
    {
        Task<int> SyncProduct();
    }
    public class ProductRepository : IProductRepository
    {
        private readonly IBus _bus;
        private readonly IGatewayHttpClientService _gatewayHttpClient;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public ProductRepository(IBus bus, IGatewayHttpClientService gatewayHttpClient, IConfiguration config, IWebHostEnvironment env)
        {
            _bus = bus;
            _gatewayHttpClient = gatewayHttpClient;
            _config = config;
            _env = env;
        }
        public async Task<int> SyncProduct()
        {
            var product = await _gatewayHttpClient.ListProduct();
            if (product is not null && product.succeeded)
            {
                string rabbitHost = _config[RabbitMqAppSettingConst.Host];
                string rabbitvHost = _config[RabbitMqAppSettingConst.Vhost];
                string productSyncQueue = _config[RabbitMqAppSettingConst.productSyncQueue];
                if (_env.IsProduction())
                {
                    rabbitHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Host);
                    rabbitvHost = Environment.GetEnvironmentVariable(RabbitMqEnvConst.Vhost);
                    productSyncQueue = Environment.GetEnvironmentVariable(RabbitMqEnvConst.productSyncQueue);
                }
                Uri uri = new Uri($"rabbitmq://{rabbitHost}/{rabbitvHost}/{productSyncQueue}");
                var endPoint = await _bus.GetSendEndpoint(uri);
                foreach (var item in product.data)
                {
                    await endPoint.Send(item);
                }
                return product.data.Count;
            }
            return 0;
        }
    }
}
