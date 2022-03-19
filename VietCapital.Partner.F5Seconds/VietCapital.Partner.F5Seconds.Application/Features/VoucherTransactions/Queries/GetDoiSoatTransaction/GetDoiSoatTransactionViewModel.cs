using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetDoiSoatTransaction
{
    public class GetDoiSoatTransactionViewModel
    {
        public List<VoucherTransaction> DoiSoatKhop { get; set; }
        public List<VoucherTransaction> DoiSoatKhongKhopF5s { get; set; }
        public List<VoucherTransaction> DoiSoatKhongKhopBvb { get; set; }
    }
}
