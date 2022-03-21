using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.WebApi.HostedService
{
    public class LoadTransactionIdToCacheHostedService : IHostedService, IDisposable
    {
        private Timer _timer = null!;
        private readonly ILogger<LoadTransactionIdToCacheHostedService> _logger;
        private readonly IServiceProvider _service;
        public LoadTransactionIdToCacheHostedService(
            ILogger<LoadTransactionIdToCacheHostedService> logger,
            IServiceProvider service)
        {
            _logger = logger;
            _service = service;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async Task DoWork()
        {
            using (var scope = _service.CreateScope())
            {
                var transService = scope.ServiceProvider.GetRequiredService<IVoucherTransactionRepositoryAsync>();
                await transService.LoadTransactionToCache();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                await DoWork();

            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("LoadProductToCache Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
