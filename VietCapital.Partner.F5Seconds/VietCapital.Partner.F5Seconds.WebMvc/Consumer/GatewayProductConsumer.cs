using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.WebMvc.Consumer
{
    public class GatewayProductConsumer : IConsumer<Product>
    {
        private readonly ILogger<GatewayProductConsumer> _logger;
        public GatewayProductConsumer(ILogger<GatewayProductConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<Product> context)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
        }
    }
}
