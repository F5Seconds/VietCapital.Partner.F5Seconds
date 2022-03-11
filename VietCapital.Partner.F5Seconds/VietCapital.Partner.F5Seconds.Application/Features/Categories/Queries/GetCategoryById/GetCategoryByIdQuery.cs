using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Response<Category>>
    {
        public int Id { get; set; }
    }
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Response<Category>>
    {
        private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
        private readonly IMapper _mapper;
        public GetCategoryByIdQueryHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
        {
            _categoryRepositoryAsync = categoryRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepositoryAsync.GetByIdAsync(request.Id);
            if (categories == null)  throw new ApiException($"Category Not Found.");
            return new Response<Category>(categories);
        }
    }
}
