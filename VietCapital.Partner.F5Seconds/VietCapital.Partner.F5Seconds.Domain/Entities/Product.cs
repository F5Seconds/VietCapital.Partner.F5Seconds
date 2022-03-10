using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public float Price { get; set; }
        public float? Point { get; set; }
        public int Type { get; set; } = 1;
        public int? Size { get; set; } = 0;
        public string Partner { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        public virtual ICollection<VoucherTransaction> VoucherTransactions { get; set; }
    }
}
