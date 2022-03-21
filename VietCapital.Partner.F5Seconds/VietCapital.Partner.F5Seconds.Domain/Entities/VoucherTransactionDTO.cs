using System;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    public class VoucherTransactionDTO : AuditableBaseEntity
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
        public string UsedBrand { get; set; }
        public DateTime? Created { get; set; }

        // public string  UsedBy { get; set; }
        public virtual Product Product { get; set; }
    }
}
