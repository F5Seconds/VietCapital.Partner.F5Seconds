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

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.DeleteCategoryByIdCommand
{
    public class DeleteCategoryByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, Response<int>>
        {
            private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
            private readonly IMapper _mapper;
            public DeleteCategoryByIdCommandHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
            {
                _categoryRepositoryAsync = categoryRepositoryAsync;
                _mapper = mapper;
            }
            public async Task<Response<int>> Handle(DeleteCategoryByIdCommand command, CancellationToken cancellationToken)
            {
                var category = await _categoryRepositoryAsync.GetByIdAsync(command.Id);
                if (category == null) throw new ApiException($"Category Not Found.");
                await _categoryRepositoryAsync.DeleteAsync(category);
                return new Response<int>(category.Id);
            }
        }
    }
}
