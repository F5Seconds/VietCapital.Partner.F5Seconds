using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.F5seconds;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<object>>
    {
        public int Id { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<object>>
    {
        private readonly IProductRepositoryAsync _productRepositoryAsync;
        private readonly IMapper _mapper;
        public GetProductByIdQueryHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
        {
            _productRepositoryAsync = productRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepositoryAsync.GetProductByIdAsync(request.Id);
            if (product == null)  throw new ApiException($"product Not Found.");
            return new Response<object>(_mapper.Map<F5ProductOutSideResponse>(product));
        }
    }
}
