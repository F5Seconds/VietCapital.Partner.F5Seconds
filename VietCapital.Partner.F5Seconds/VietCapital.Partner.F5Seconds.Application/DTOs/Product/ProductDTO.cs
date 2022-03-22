using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Domain.Common;

namespace VietCapital.Partner.F5Seconds.Domain.Entities
{
    public class ProductDTO : Product
    {
        public List<F5sVoucherOffice> StoreList {get;set;}
    }
}
