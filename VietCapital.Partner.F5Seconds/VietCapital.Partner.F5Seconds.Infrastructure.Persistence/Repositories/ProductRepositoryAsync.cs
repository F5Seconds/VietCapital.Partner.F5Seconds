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
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;

        public ProductRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Product>();
        }

        public async Task<Product> FindByCodeAsync(string code)
        {
            return await _products
                .Include(cp => cp.CategoryProducts)
                .ThenInclude(c => c.Category)
                .SingleOrDefaultAsync(x => x.Code.Equals(code.Trim()) && x.Status);
        }

        public async Task<IReadOnlyList<Product>> GetListAsync()
        {
            return await _products
                .Include(cp => cp.CategoryProducts)
                .ThenInclude(c => c.Category)
                .Where(p => p.Status)
                .ToListAsync();
        }

        public async Task<bool> IsExitedByCode(string code)
        {
            return await _products.AnyAsync(x => x.Code.Equals(code.Trim()));
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _products
                .AnyAsync(p => p.Code == barcode);
        }
    }
}
