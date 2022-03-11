using System;
using System.Collections.Generic;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesViewModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}
