using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.WebApp.Consumer
{
    public class ChannelUpdateStateConsumer : IConsumer<ChannelUpdateStateDto>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        private readonly ILogger<ChannelUpdateStateConsumer> _logger;
        public ChannelUpdateStateConsumer(
            IVoucherTransactionRepositoryAsync voucherTransaction, ILogger<ChannelUpdateStateConsumer> logger)
        {

            _voucherTransaction = voucherTransaction;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ChannelUpdateStateDto> context)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(context.Message));
            var message = context.Message;
            var trans = await _voucherTransaction.GetVoucherByTransId(message.TransactionId);
            _logger.LogInformation($"######## {JsonConvert.SerializeObject(trans)}");
            if (trans is not null)
            {
                trans.State = message.State;
                trans.UsedTime = message.UsedTime;
                trans.UsedBrand = message.UsedBrand;
                await _voucherTransaction.UpdateAsync(trans);
            }
        }
    }
}
