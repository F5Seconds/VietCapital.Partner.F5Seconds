using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;

namespace VietCapital.Partner.F5Seconds.Application.Features.Transactions.Commands
{
    public class CreateVoucherTransactionCommandValidator : AbstractValidator<CreateVoucherTransactionCommand> 
    {
        private readonly IVoucherTransactionRepositoryAsync _voucherTransaction;
        private readonly IProductRepositoryAsync _productRepository;
        public CreateVoucherTransactionCommandValidator(
            IVoucherTransactionRepositoryAsync voucherTransaction,
            IProductRepositoryAsync productRepository)
        {
            _voucherTransaction = voucherTransaction;
            _productRepository = productRepository;
            RuleFor(p => p.transactionId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueTransId).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.productCode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsExitedProduct).WithMessage("{PropertyName} not exists.");

            RuleFor(p => p.customerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.quantity)
                .NotNull()
                .GreaterThan(0);
        }

        private async Task<bool> IsUniqueTransId(string transId, CancellationToken cancellationToken)
        {
            var result = await _voucherTransaction.IsUniqueTransactionAsync(transId);
            return !result;
        }

        private async Task<bool> IsExitedProduct(string productCode, CancellationToken cancellationToken)
        {
            return await _productRepository.IsExitedByCode(productCode);
        }
    }
}
