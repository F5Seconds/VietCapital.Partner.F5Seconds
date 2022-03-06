using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.WebMvc.ModelView
{
    public class BuyVoucher
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Cif { get; set; }
    }

    public class ListVoucher
    {
        public string Cif { get; set; }
        public int State { get; set; }
        public List<VoucherTransaction> Vouchers { get; set; }
    }

    public class DetailVoucher
    {
        public VoucherTransaction Voucher { get; set; }
        public Byte[] QrCodeVoucher { get; set; }
    }
}
