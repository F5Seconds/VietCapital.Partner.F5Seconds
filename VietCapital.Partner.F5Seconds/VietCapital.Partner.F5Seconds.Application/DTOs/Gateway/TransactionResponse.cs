using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.Gateway
{
    public class TransactionOutSideResponse
    {
        public int ProductId { get; set; }
        public string TransactionId { get; set; }
        public float ProductPrice { get; set; }
        public string CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public string VoucherCode { get; set; }
        public int State { get; set; } = 1;
        public DateTime ExpiryDate { get; set; }
        public DateTime? UsedTime { get; set; }
        public DateTime? Created { get; set; }
        public string UsedBrand { get; set; }
        public virtual ProductInSideResponse Product { get; set; }
    }
}
