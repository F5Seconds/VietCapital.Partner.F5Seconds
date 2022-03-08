using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct;
using VietCapital.Partner.F5Seconds.Application.Filters;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
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
                .SingleOrDefaultAsync(x => x.ProductCode.Equals(code.Trim()) && x.Status);
        }

        public async Task<IReadOnlyList<Product>> GetListAsync()
        {
            return await _products
                .Include(cp => cp.CategoryProducts)
                .ThenInclude(c => c.Category)
                .Where(p => p.Status)
                .ToListAsync();
        }

        private void Search(ref IQueryable<Product> products, string search)
        {
            if (string.IsNullOrWhiteSpace(search)) return;
            search = $"%{search.Trim()}%";
            products = products.Where(x =>
                EF.Functions.Like(x.Name, search) ||
                EF.Functions.Like(x.Point, search) ||
                EF.Functions.Like(x.BrandName, search) ||
                EF.Functions.Like(x.ProductCode, search)
            );
        }

        public async Task<PagedList<Product>> GetPagedListAsync(GetListProductParameter parameter)
        {
            var products = _products.Include(cp => cp.CategoryProducts)
                .ThenInclude(c => c.Category)
                .Where(p => p.Status).AsQueryable();
            Search(ref products,parameter.Search);
            return await PagedList<Product>.ToPagedList(products.OrderByDescending(x => x.Id).AsNoTracking(), parameter.PageNumber, parameter.PageSize);
        }

        public async Task<bool> IsExitedByCode(string code)
        {
            return await _products.AnyAsync(x => x.ProductCode.Equals(code.Trim()));
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _products
                .AnyAsync(p => p.ProductCode == barcode);
        }
    }
}
