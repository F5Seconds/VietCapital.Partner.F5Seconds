using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetProductByName
{
    public class GetProductByNameQuery : IRequest<Response<IEnumerable<Product>>>
    {
        public string Name { get; set; }
        public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, Response<IEnumerable<Product>>>
        {
            private readonly IProductRepositoryAsync _productRepository;
            public GetProductByNameQueryHandler(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<IEnumerable<Product>>> Handle(GetProductByNameQuery query, CancellationToken cancellationToken)
            {
                //var product = await _productRepository.GetByNameAsync(query.Name);
                //if (product == null) throw new ApiException($"Product Not Found.");
                return new Response<IEnumerable<Product>>(null);
            }
        }
    }
}
