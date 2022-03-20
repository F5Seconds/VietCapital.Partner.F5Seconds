using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory
{
    public class GetDetailCategoryQuery : IRequest<Response<CategoryOutsideResponse>>
    {
        public int? Id { get; set; }
        public class GetDetailCategoryQueryHandler : IRequestHandler<GetDetailCategoryQuery, Response<CategoryOutsideResponse>>
        {
            private readonly ICategoryRepositoryAsync _category;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _distributedCache;
            public GetDetailCategoryQueryHandler(ICategoryRepositoryAsync category, IMapper mapper, IDistributedCache distributedCache)
            {
                _category = category;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }
            public async Task<Response<CategoryOutsideResponse>> Handle(GetDetailCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _category.FindCategoryById(request.Id??0);
                if (category == null) return new Response<CategoryOutsideResponse>(false,null, ResponseConst.NotData);
                var serializedCategory = JsonConvert.SerializeObject(category, Formatting.Indented);
                return new Response<CategoryOutsideResponse>(true, _mapper.Map<CategoryOutsideResponse>(category));
            }
        }
    }
}
