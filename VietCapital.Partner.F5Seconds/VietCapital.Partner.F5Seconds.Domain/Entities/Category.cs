using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    public class Category: AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
