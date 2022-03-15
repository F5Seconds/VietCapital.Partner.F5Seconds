using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds
{
    public class F5ProductOutSideResponse : F5ProductInSideResponse
    {
        public virtual ICollection<F5sVoucherOffice> StoreList { get; set; }
        public virtual ICollection<CategoryInsideResponse> Categories { get; set; }
    }

    public class F5ProductInSideResponse
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public float? Point { get; set; }
        public float Price { get; set; }
        public int Type { get; set; } = 1;
        public string Partner { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool Status { get; set; }

    }
}
