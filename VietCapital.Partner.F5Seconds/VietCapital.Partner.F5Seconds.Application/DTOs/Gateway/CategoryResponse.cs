using System.Collections.Generic;

namespace VietCapital.Partner.F5Seconds.Application.DTOs.Gateway
{
    public class CategoryInsideResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }

    public class CategoryOutsideResponse : CategoryInsideResponse
    {
        public virtual ICollection<ProductInSideResponse> Products { get; set; }
    }
}
