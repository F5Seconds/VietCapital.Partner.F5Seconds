using System.ComponentModel.DataAnnotations;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct
{
    public class GetDetailProductParameter
    {
        [Required]
        public string ProductCode { get; set; }
    }
}
