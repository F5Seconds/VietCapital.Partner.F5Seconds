using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct
{
    public class GetDetailProductQuery : IRequest<Response<ProductOutSideResponse>>
    {
        public string ProductCode { get; set; }
        public class GetDetailProductQueryHandler : IRequestHandler<GetDetailProductQuery, Response<ProductOutSideResponse>>
        {
            private readonly IProductRepositoryAsync _product;
            private readonly IMapper _mapper;
            private readonly IGatewayHttpClientService _gatewayHttpClient;
            public GetDetailProductQueryHandler(IProductRepositoryAsync product, IMapper mapper, IGatewayHttpClientService gatewayHttpClient)
            {
                _product = product;
                _mapper = mapper;
                _gatewayHttpClient = gatewayHttpClient; 
            }
            public async Task<Response<ProductOutSideResponse>> Handle(GetDetailProductQuery request, CancellationToken cancellationToken)
            {
                var product = await _product.FindByCodeAsync(request.ProductCode);
                if (product == null) return new Response<ProductOutSideResponse>(false, null, ResponseConst.NotData);
                var pGateway = await _gatewayHttpClient.DetailProduct(request.ProductCode);
                if (!pGateway.Succeeded) return new Response<ProductOutSideResponse>(false,null,pGateway.Message,pGateway.Errors);
                if(pGateway.Data is null) return new Response<ProductOutSideResponse>(false, null, ResponseConst.PartnerNotData);
                return new Response<ProductOutSideResponse>(true,_mapper.Map<ProductOutSideResponse>(product,opt => opt.AfterMap((src,dest) => dest.StoreList = pGateway.Data.storeList)));
            }
        }
    }
}
