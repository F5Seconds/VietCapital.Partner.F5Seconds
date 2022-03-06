using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Const;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory
{
    public class GetDetailCategoryQuery : IRequest<Response<Category>>
    {
        public int? Id { get; set; }
        public class GetDetailCategoryQueryHandler : IRequestHandler<GetDetailCategoryQuery, Response<Category>>
        {
            private readonly ICategoryRepositoryAsync _category;
            public GetDetailCategoryQueryHandler(ICategoryRepositoryAsync category)
            {
                _category = category;
            }
            public async Task<Response<Category>> Handle(GetDetailCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _category.FindCategoryById(request.Id??0);
                if (category == null) return new Response<Category>(false,null, ResponseConst.NotData);
                return new Response<Category>(true,category);
            }
        }
    }
}
