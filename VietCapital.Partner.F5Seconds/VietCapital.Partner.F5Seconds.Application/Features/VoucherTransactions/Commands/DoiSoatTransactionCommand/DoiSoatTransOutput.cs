using System;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Commands.DoiSoatTransactionCommand
{
    public class DoiSoatTransOutput
    {
        public string TransactionId { get; set; }
        public int ProductId { get; set; }
        public string VoucherCode { get; set; }
        public int State { get; set; }
        public string CustomerId { get; set; }
        public DateTime Created { get; set; }
    }
}
