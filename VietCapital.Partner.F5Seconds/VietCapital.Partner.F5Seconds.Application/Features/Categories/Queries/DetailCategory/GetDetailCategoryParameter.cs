using System.ComponentModel.DataAnnotations;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory
{
    public class GetDetailCategoryParameter
    {
        [Required]
        public int Id { get; set; }
    }
}
