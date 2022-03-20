using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Commands.DoiSoatTransactionCommand
{
    public class DoiSoatTransactionViewModel
    {
        public List<DoiSoatTransOutput> DoiSoatKhop { get; set; }
        public List<DoiSoatTransOutput> DoiSoatKhongKhopF5s { get; set; }
        public List<DoiSoatTransOutput> DoiSoatKhongKhopBvb { get; set; }
    }
}
