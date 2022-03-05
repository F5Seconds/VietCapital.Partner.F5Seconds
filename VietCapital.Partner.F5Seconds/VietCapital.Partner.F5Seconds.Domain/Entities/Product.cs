using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        [ReadOnly(true)]
        public string Code { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        [ReadOnly(true)]
        public float Price { get; set; }
        public float? Point { get; set; }
        [ReadOnly(true)]
        public int Type { get; set; } = 1;
        public int Size { get; set; } = 0;
        [ReadOnly(true)]
        public string Partner { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        public virtual ICollection<VoucherTransaction> VoucherTransactions { get; set; }
    }
}
