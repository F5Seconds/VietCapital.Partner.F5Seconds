using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VietCapital.Partner.F5Seconds.WebApi.HostedService
{
    public class RedisCacheHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly ILogger<RedisCacheHostedService> _logger;
        public RedisCacheHostedService(ILogger<RedisCacheHostedService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                Publisher();
            }, cancellationToken);
            
            return Task.CompletedTask;
        }

        private Task Publisher()
        {
            using ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            ISubscriber sub = redis.GetSubscriber();
            for (int i = 0; i <= 10; i++)
            {
                sub.Publish("VietcapitalMessage", $"Message {i}");
                Console.WriteLine($"Message {i} published successfully");
                Thread.Sleep(2000);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
