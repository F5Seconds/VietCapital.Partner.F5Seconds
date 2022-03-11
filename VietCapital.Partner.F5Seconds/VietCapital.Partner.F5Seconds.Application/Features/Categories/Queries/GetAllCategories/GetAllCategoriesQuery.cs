using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
    }
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Response<object>>
    {
        private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
        private readonly IMapper _mapper;
        public GetAllCategoriesQueryHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
        {
            _categoryRepositoryAsync = categoryRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<object>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var filter = _mapper.Map<GetAllCategoriesParameter>(request);
            var categories = await _categoryRepositoryAsync.GetAllPagedListAsync(filter);
            if (categories == null) return new Response<object>(false, null, ResponseConst.NotData);
            return new Response<object>(true, new
            {
                categories.CurrentPage,
                categories.TotalPages,
                categories.PageSize,
                categories.TotalCount,
                categories.HasPrevious,
                categories.HasNext,
                Data = _mapper.Map<ICollection<CategoryOutsideResponse>>(categories),
            });
        }
    }
}
