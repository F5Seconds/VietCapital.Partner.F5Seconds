using FluentValidation;

namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.DetailCategory
{
    public class GetDetailCategoryQueryValidator : AbstractValidator<GetDetailCategoryQuery>
    {
        public GetDetailCategoryQueryValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0)
                .NotNull();
        }
    }
}
