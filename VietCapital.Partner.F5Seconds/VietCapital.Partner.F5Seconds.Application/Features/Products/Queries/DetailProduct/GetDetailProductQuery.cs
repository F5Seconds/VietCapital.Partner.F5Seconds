using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct
{
    public class GetDetailProductQuery : IRequest<Response<Product>>
    {
        public string Code { get; set; }
        public class GetDetailProductQueryHandler : IRequestHandler<GetDetailProductQuery, Response<Product>>
        {
            private readonly IProductRepositoryAsync _product;
            public GetDetailProductQueryHandler(IProductRepositoryAsync product)
            {
                _product = product;
            }
            public async Task<Response<Product>> Handle(GetDetailProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _product.FindByCodeAsync(request.Code);
                if (product == null) return new Response<Product>(false, null, ResponseConst.NotData);
                return new Response<Product>(true,product);
            }
        }
    }
}
