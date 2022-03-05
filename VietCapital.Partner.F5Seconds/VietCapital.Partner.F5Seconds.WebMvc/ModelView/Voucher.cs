using System;
using System.ComponentModel.DataAnnotations;

namespace VietCapital.Partner.F5Seconds.WebMvc.ModelView
{
    public class BuyVoucher
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Cif { get; set; }
    }
}
