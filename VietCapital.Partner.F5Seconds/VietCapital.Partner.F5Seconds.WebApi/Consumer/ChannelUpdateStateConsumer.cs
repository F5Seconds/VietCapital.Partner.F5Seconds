using MassTransit;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.WebApi.Consumer
{
    public class ChannelUpdateStateConsumer : IConsumer<ChannelUpdateStateDto>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        public ChannelUpdateStateConsumer(
            IVoucherTransactionRepositoryAsync voucherTransaction)
        {

            _voucherTransaction = voucherTransaction;
  
        }
        public async Task Consume(ConsumeContext<ChannelUpdateStateDto> context)
        {
            var message = context.Message;
            var trans = await _voucherTransaction.GetVoucherByTransId(message.TransactionId);
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
