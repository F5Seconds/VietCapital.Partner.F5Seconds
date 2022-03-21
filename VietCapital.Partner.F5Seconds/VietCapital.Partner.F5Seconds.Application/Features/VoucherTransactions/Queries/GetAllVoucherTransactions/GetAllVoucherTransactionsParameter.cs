
using System;
using VietCapital.Partner.F5Seconds.Application.Filters;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetAllVoucherTransactions
{
    public class GetAllVoucherTransactionsParameter : RequestParameter
    {
        public DateTime? From {get;set;}
        public DateTime? To {get;set;}

    }
}
