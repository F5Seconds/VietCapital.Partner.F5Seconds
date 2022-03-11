using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetAllCategories;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repository;

namespace VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Repositories
{
    public class CategoryRepositoryAsync : GenericRepositoryAsync<Category>, ICategoryRepositoryAsync
    {
        private readonly DbSet<Category> _categories;
        public CategoryRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _categories = dbContext.Set<Category>();
        }

        public async Task<Category> FindCategoryById(int id)
        {
            var cats = _categories
                .Include(cp => cp.CategoryProducts)
                .ThenInclude(p => p.Product).AsQueryable();
            return await cats.SingleOrDefaultAsync(x => x.Id.Equals(id) && x.Status);
        }

        public async Task<PagedList<Category>> GetAllPagedListAsync(GetAllCategoriesParameter parameter)
        {
            var categories = _categories
                .Include(cp => cp.CategoryProducts.Where(p => p.Product.Status))
                .ThenInclude(p => p.Product)
                .Where(c => c.Status).AsQueryable();
            Search(ref categories,parameter.Search);
            return await PagedList<Category>.ToPagedList(categories.OrderByDescending(x => x.Id).AsNoTracking(), parameter.PageNumber, parameter.PageSize);
        }

        public async Task<List<Category>> GetByNameAsync(string Name)
        {
           return await _categories.Where(c =>c.Name.ToLower().Contains(Name??"".ToLower())).AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<Category>> GetCategoryList()
        {
            return await _categories
                .Include(cp => cp.CategoryProducts.Where(p => p.Product.Status))
                .ThenInclude(p => p.Product)
                .Where(c => c.Status).ToListAsync();
        }

        public async Task<PagedList<Category>> GetPagedListAsync(GetListCategoryParameter parameter)
        {
            var categories = _categories
                .Include(cp => cp.CategoryProducts.Where(p => p.Product.Status))
                .ThenInclude(p => p.Product)
                .Where(c => c.Status).AsQueryable();
            Search(ref categories,parameter.Search);
            return await PagedList<Category>.ToPagedList(categories.OrderByDescending(x => x.Id).AsNoTracking(), parameter.PageNumber, parameter.PageSize);
        }

        private void Search(ref IQueryable<Category> products, string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return;
            search = $"%{search.Trim()}%";
            products = products.Where(x =>
                EF.Functions.Like(x.Name, search)
            );
        }
    }
}
