using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoriesCommandValidator : AbstractValidator<CreateCategoriesCommand> 
    {
        private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;

        public CreateCategoriesCommandValidator( ICategoryRepositoryAsync CategoryRepositoryAsync)
        {
            _categoryRepositoryAsync = CategoryRepositoryAsync;
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
