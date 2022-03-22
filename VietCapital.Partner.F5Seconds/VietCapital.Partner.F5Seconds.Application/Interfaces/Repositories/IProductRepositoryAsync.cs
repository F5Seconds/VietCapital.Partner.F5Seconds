using System.Collections.Generic;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetAllProducts;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<IReadOnlyList<Product>> GetListAsync();
        Task<PagedList<Product>> GetPagedListAsync(GetListProductParameter parameter);
        Task<Product> FindByCodeAsync(string code);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> IsExitedByCode(string code);
        Task<bool> IsUniqueBarcodeAsync(string barcode);
        Task<PagedList<Product>> GetAllPagedListAsync(GetAllProductsParameter parameter);
        Task<List<CategoryProduct>> GetProductInCategoryByIdAsync(int id);
        Task<string[]> GetAllProductCode();
        Task LoadProductCodeToCache();

    }
}
