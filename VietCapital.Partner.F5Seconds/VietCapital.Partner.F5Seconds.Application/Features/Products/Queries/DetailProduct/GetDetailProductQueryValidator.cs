using FluentValidation;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.DetailProduct
{
    public class GetDetailProductQueryValidator: AbstractValidator<GetDetailProductQuery>
    {
        public GetDetailProductQueryValidator()
        {
            RuleFor(p => p.ProductCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
