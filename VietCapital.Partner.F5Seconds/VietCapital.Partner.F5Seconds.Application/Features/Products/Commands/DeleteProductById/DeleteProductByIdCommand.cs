using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.DeleteProductByIdCommand
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepositoryAsync;
            private readonly IMapper _mapper;
            public DeleteProductByIdCommandHandler(IProductRepositoryAsync productRepositoryAsync, IMapper mapper)
            {
                _productRepositoryAsync = productRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepositoryAsync.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                await _productRepositoryAsync.DeleteAsync(product);
                return new Response<int>(product.Id);
            }
        }
    }
}
