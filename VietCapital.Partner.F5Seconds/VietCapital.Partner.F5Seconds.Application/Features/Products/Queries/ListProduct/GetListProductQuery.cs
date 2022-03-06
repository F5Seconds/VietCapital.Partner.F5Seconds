using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct
{
    public class GetListProductQuery : IRequest<Response<IReadOnlyList<Product>>>
    {
        public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, Response<IReadOnlyList<Product>>>
        {
            private readonly IProductRepositoryAsync _products;
            public GetListProductQueryHandler(IProductRepositoryAsync products)
            {
                _products = products;
            }
            public async Task<Response<IReadOnlyList<Product>>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
            {
                var products = await _products.GetListAsync();
                if (products is null) return new Response<IReadOnlyList<Product>>(false, null, ResponseConst.NotData);
                return new Response<IReadOnlyList<Product>>(true,products);
            }
        }
    }
}
