using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory
{
    public class GetListCategoryQuery : IRequest<Response<IReadOnlyList<Category>>>
    {
        public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, Response<IReadOnlyList<Category>>>
        {
            private readonly ICategoryRepositoryAsync _category;
            public GetListCategoryQueryHandler(ICategoryRepositoryAsync category)
            {
                _category = category;
            }
            public async Task<Response<IReadOnlyList<Category>>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
            {
                var categories = await _category.GetCategoryList();
                if(categories == null) return new Response<IReadOnlyList<Category>>(false,null, ResponseConst.NotData);
                return new Response<IReadOnlyList<Category>>(true, categories);
            }
        }
    }
}
