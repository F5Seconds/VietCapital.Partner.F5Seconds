using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VietCapital.Partner.F5Seconds.WebApp.HostedService
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
                Subscriber();
            }, cancellationToken);
            
            return Task.CompletedTask;
        }

        private Task Subscriber()
        {
            using ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            ISubscriber subScriber = redis.GetSubscriber();

            //Subscribe to the channel "thecode-buzz-channel"

            subScriber.Subscribe("VietcapitalMessage", (channel, message) =>
            {
                //Output received message
                _logger.LogInformation($"[{DateTime.Now:HH:mm:ss}]: {$"Message {message} received successfully"}");
            });
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
