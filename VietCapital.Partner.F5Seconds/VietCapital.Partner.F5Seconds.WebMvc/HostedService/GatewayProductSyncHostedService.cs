using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Shared.Const;

namespace VietCapital.Partner.F5Seconds.WebMvc.HostedService
{
    public class GatewayProductSyncHostedService : IHostedService, IDisposable
    {
        private readonly CrontabSchedule _crontabSchedule;
        private readonly IServiceProvider _service;
        private DateTime _nextRun;
        private string Schedule = "0 0 0 * * *";
        private Timer _timer = null!;
        private readonly IBus _bus;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GatewayProductSyncHostedService> _logger;
        private readonly IGatewayHttpClientService _gatewayHttpClient;
        private readonly IConfiguration _config;
        public GatewayProductSyncHostedService(

            ILogger<GatewayProductSyncHostedService> logger, 
            IServiceProvider service, 
            IWebHostEnvironment env, 
            IBus bus,
            IGatewayHttpClientService gatewayHttpClient,
            IConfiguration config)
        {
            _env = env;
            if (_env.IsProduction())
            {
                Schedule = Environment.GetEnvironmentVariable("SCHEDULE_PRODUCTSYNC");
            }
            _crontabSchedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
            _logger = logger;
            _service = service;
            _bus = bus;
            _gatewayHttpClient = gatewayHttpClient; 
            _config = config;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async Task DoWork()
        {
            var product = await _gatewayHttpClient.ListProduct();
            if(product is not null && product.Succeeded)
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
                foreach (var item in product.Data)
                {
                    await endPoint.Send(item);
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                //await DoWork();
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(UntilNextExecution(), cancellationToken);
                    await DoWork();
                    _nextRun = _crontabSchedule.GetNextOccurrence(DateTime.Now);
                }
            }, cancellationToken);
            return Task.CompletedTask;
        }

        private int UntilNextExecution() => Math.Max(0, (int)_nextRun.Subtract(DateTime.Now).TotalMilliseconds);
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
