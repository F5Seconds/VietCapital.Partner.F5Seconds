using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VietCapital.Partner.F5Seconds.Domain.Common
{
    public abstract class AuditableBaseEntity
    {
        public virtual int Id { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime Created { get; set; }
        [JsonIgnore]
        public string LastModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime? LastModified { get; set; }
    }
}
