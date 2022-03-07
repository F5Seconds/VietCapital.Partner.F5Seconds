using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct
{
    public class GetListProductQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; } = "";
        public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, Response<object>>
        {
            private readonly IProductRepositoryAsync _products;
            private readonly IMapper _mapper;
            public GetListProductQueryHandler(IProductRepositoryAsync products, IMapper mapper)
            {
                _products = products;
                _mapper = mapper;
            }
            public async Task<Response<object>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
            {
                var filter = _mapper.Map<GetListProductParameter>(request);
                var products = await _products.GetPagedListAsync(filter);
                if (products is null) return new Response<object>(false, null, ResponseConst.NotData);
                return new Response<object>(true, new
                {
                    products.CurrentPage,
                    products.TotalPages,
                    products.PageSize,
                    products.TotalCount,
                    products.HasPrevious,
                    products.HasNext,
                    Data = products
                });
            }
        }
    }
}
