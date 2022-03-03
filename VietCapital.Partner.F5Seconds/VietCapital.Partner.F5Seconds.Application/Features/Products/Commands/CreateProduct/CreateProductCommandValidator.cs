using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IProductRepositoryAsync productRepository;

        public CreateProductCommandValidator(IProductRepositoryAsync productRepository)
        {
            this.productRepository = productRepository;

            // RuleFor(p => p.Barcode)
            //     .NotEmpty().WithMessage("{PropertyName} is required.")
            //     .NotNull()
            //     .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
            //     .MustAsync(IsUniqueBarcode).WithMessage("{PropertyName} already exists.");

            // RuleFor(p => p.Name)
            //     .NotEmpty().WithMessage("{PropertyName} is required.")
            //     .NotNull()
            //     .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        }

        // private async Task<bool> IsUniqueBarcode(string barcode, CancellationToken cancellationToken)
        // {
        //     return await productRepository.IsUniqueBarcodeAsync(barcode);
        // }
    }
}
