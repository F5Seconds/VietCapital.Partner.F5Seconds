using FluentValidation;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter
{
    public class GetVoucherTransFilterQueryValidator : AbstractValidator<GetVoucherTransFilterQuery>
    {
        public GetVoucherTransFilterQueryValidator()
        {
            RuleFor(p => p.Cif)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(p => p.State)
                .NotNull()
                .GreaterThan(0);
        }
    }
}
