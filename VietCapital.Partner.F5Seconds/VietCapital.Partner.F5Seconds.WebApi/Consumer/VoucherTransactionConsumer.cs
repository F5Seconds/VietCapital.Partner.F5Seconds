using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.WebApi.Consumer
{
    public class VoucherTransactionConsumer : IConsumer<VoucherTransaction>
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        public VoucherTransactionConsumer(
            IVoucherTransactionRepositoryAsync voucherTransaction)
        {
            _voucherTransaction = voucherTransaction;
        }
        public async Task Consume(ConsumeContext<VoucherTransaction> context)
        {
            await _voucherTransaction.AddAsync(context.Message);
        }
    }
}
