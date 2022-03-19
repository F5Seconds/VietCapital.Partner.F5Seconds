using FluentValidation;

namespace VietCapital.Partner.F5Seconds.Application.Features.VoucherTransactions.Queries.GetDoiSoatTransaction
{
    public class GetDoiSoatTransactionQueryValidator : AbstractValidator<GetDoiSoatTransactionQuery>
    {
        public GetDoiSoatTransactionQueryValidator()
        {
            RuleFor(p => p.NgayBatDau)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.NgayKetThuc)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
