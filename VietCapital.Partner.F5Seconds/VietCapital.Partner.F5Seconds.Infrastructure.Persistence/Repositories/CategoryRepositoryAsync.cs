using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
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

        public async Task<IReadOnlyList<Category>> GetCategoryList()
        {
            return await _categories
                .Include(cp => cp.CategoryProducts.Where(p => p.Product.Status))
                .ThenInclude(p => p.Product)
                .Where(c => c.Status).ToListAsync();
        }
    }
}
