﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
        Task<bool> IsExitedByCode(string code);
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
