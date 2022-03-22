using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    [DataContract(IsReference = true)]
    public class CategoryProduct : AuditableBaseEntity
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Product Product { get; set; }
        
    }
}
