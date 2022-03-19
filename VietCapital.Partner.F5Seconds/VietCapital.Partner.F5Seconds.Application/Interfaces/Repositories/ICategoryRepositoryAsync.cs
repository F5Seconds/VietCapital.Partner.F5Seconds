using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetAllCategories;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface ICategoryRepositoryAsync : IGenericRepositoryAsync<Category>
    {
        Task<IReadOnlyList<Category>> GetCategoryList();
        Task<Category> FindCategoryById(int id);
        Task<PagedList<Category>> GetPagedListAsync(GetListCategoryParameter parameter);
        Task<PagedList<Category>> GetAllPagedListAsync(GetAllCategoriesParameter parameter);
        Task<List<Category>> GetByNameAsync(string Name);


    }
}
