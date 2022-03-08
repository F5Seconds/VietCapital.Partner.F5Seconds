using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory
{
    public class GetListCategoryQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, Response<object>>
        {
            private readonly ICategoryRepositoryAsync _category;
            private readonly IMapper _mapper;
            public GetListCategoryQueryHandler(ICategoryRepositoryAsync category, IMapper mapper)
            {
                _mapper = mapper;
                _category = category;
            }
            public async Task<Response<object>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
            {
                var filter = _mapper.Map<GetListCategoryParameter>(request);
                var categories = await _category.GetPagedListAsync(filter);
                if(categories == null) return new Response<object>(false,null, ResponseConst.NotData);
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
}
