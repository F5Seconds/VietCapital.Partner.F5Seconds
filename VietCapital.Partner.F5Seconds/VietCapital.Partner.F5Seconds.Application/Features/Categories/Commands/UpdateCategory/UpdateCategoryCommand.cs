using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }
        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<int>>
        {
            private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
            private readonly IMapper _mapper;
            public UpdateCategoryCommandHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
            {
                _categoryRepositoryAsync = categoryRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
            {
                var category = await _categoryRepositoryAsync.GetByIdAsync(command.Id);
                if (category == null)
                {
                    throw new ApiException($"Category Not Found.");
                }
                else
                {
                    category.Name = command.Name;
                    category.Image = command.Image;
                    category.Status = command.Status;
                    category.CategoryProducts = command.CategoryProducts;
                    await _categoryRepositoryAsync.UpdateAsync(category);
                    return new Response<int>(category.Id);
                }
            }
        }
    }
}
