using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.WebApp.Consumer
{
    public class GatewayProductConsumer : IConsumer<Product>
    {
        private readonly ILogger<GatewayProductConsumer> _logger;
        private readonly IProductRepositoryAsync _productRepository;
        public GatewayProductConsumer(ILogger<GatewayProductConsumer> logger, IProductRepositoryAsync productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            var exited = await _productRepository.IsUniqueBarcodeAsync(context.Message.ProductCode);
            if (!exited)
            {
                await _productRepository.AddAsync(context.Message);
            }
        }
    }
}
