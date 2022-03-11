using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoriesCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }
        public class CreateCategoriesCommandHandler : IRequestHandler<CreateCategoriesCommand, Response<int>>
        {
            private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
            private readonly IMapper _mapper;
            public CreateCategoriesCommandHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
            {
                _categoryRepositoryAsync = categoryRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(CreateCategoriesCommand request, CancellationToken cancellationToken)
            {
                var category = _mapper.Map<Category>(request);
                await _categoryRepositoryAsync.AddAsync(category);
                return new Response<int>(category.Id);
            }
        }
    }
}
