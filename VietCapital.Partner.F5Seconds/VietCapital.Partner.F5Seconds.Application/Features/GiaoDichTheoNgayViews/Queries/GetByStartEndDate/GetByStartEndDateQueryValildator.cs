using FluentValidation;

namespace VietCapital.Partner.F5Seconds.Application.Features.GiaoDichTheoNgayViews.Queries.GetByStartEndDate
{
    public class GetByStartEndDateQueryValildator : AbstractValidator<GetByStartEndDateQuery>
    {
        public GetByStartEndDateQueryValildator()
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
