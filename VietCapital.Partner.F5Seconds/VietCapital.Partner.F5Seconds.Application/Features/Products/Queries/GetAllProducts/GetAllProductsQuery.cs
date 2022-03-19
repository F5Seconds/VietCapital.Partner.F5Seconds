using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Response<object>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<GetAllProductsParameter>(request);
            var products = await _productRepositoryAsync.GetAllPagedListAsync(filter);
            if (products == null) return new Response<object>(false, null, ResponseConst.NotData);
            return new Response<object>(true, new
            {
                products.CurrentPage,
                products.TotalPages,
                products.PageSize,
                products.TotalCount,
                products.HasPrevious,
                products.HasNext,
                Data = _mapper.Map<ICollection<F5ProductOutSideResponse>>(products),
            });
        }
    }
}
