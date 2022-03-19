using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Response<int>>
    {
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public float Price { get; set; }
        public float? Point { get; set; }
        public int Type { get; set; } = 1;
        public int? Size { get; set; } = 0;
        public string Partner { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool Status { get; set; }
        public  List<CategoryProduct> CategoryProducts { get; set; }
        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepositoryAsync;
            private readonly IMapper _mapper;
            public CreateProductCommandHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
            {
                _productRepositoryAsync = productRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request);
                await _productRepositoryAsync.AddAsync(product);
                return new Response<int>(product.Id);
            }
        }
    }
}
