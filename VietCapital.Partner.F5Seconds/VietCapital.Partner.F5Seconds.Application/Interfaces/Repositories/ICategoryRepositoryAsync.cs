using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface ICategoryRepositoryAsync : IGenericRepositoryAsync<Category>
    {
        Task<IReadOnlyList<Category>> GetCategoryList();
        Task<Category> FindCategoryById(int id);
    }
}
