using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct
{
    public class GetDetailProductQuery : IRequest<Response<ProductOutSideResponse>>
    {
        public string Code { get; set; }
        public class GetDetailProductQueryHandler : IRequestHandler<GetDetailProductQuery, Response<ProductOutSideResponse>>
        {
            private readonly IProductRepositoryAsync _product;
            private readonly IMapper _mapper;
            public GetDetailProductQueryHandler(IProductRepositoryAsync product, IMapper mapper)
            {
                _product = product;
                _mapper = mapper;
            }
            public async Task<Response<ProductOutSideResponse>> Handle(GetDetailProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _product.FindByCodeAsync(request.Code);
                if (product == null) return new Response<ProductOutSideResponse>(false, null, ResponseConst.NotData);
                return new Response<ProductOutSideResponse>(true,_mapper.Map<ProductOutSideResponse>(product));
            }
        }
    }
}
