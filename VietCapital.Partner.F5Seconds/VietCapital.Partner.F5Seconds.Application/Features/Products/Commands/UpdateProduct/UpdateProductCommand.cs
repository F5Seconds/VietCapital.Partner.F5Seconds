using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Exceptions;
using VietCapital.Partner.F5Seconds.Application.Interfaces.Repositories;
using VietCapital.Partner.F5Seconds.Application.Wrappers;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Term { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public float Price { get; set; }
        public float? Point { get; set; }
        public int Type { get; set; } = 1;
        public int? Size { get; set; } = 0;
        public string Partner { get; set; }
        public string BrandName { get; set; }
        public string BrandLogo { get; set; }
        public bool Status { get; set; }
        public List<CategoryProduct> CategoryProducts { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<int>>
        {
            private readonly IProductRepositoryAsync _productRepositoryAsync;
            private readonly ICategoryProductRepositoryAsync _CategoryProductRepositoryAsync;

            private readonly IMapper _mapper;
            public UpdateProductCommandHandler(IProductRepositoryAsync productRepositoryAsync,ICategoryProductRepositoryAsync CategoryProductRepositoryAsync, IMapper mapper)
            {
                _productRepositoryAsync = productRepositoryAsync;
                _CategoryProductRepositoryAsync = CategoryProductRepositoryAsync;
                _mapper = mapper;
            }

            public async Task<Response<int>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {

                await _productRepositoryAsync.BeginTransactionAsync();
                try
                {
                    var product = await _productRepositoryAsync.GetByIdAsync(command.Id);
                    if (product == null)
                    {
                        throw new ApiException($"Category Not Found.");
                    }
                    else
                    {
                        var OldCategoryProduct = await _productRepositoryAsync.GetProductInCategoryByIdAsync(command.Id);
                        await _CategoryProductRepositoryAsync.DeleteRangeAsync(OldCategoryProduct);
                        product.ProductCode = command.ProductCode;
                        product.ProductId = command.ProductId;
                        product.Name = command.Name;
                        product.Content = command.Content;
                        product.Image = command.Image;
                        product.Term = command.Term;
                        product.Thumbnail = command.Thumbnail;
                        product.Price = command.Price;
                        product.Point = command.Point;
                        product.Type = command.Type;
                        product.Size = command.Size;
                        product.Partner = command.Partner;
                        product.BrandName = command.BrandName;
                        product.BrandLogo = command.BrandLogo;
                        product.Status = command.Status;
                        // product.CategoryProducts = command.CategoryProducts;
                        await _productRepositoryAsync.UpdateAsync(product);
                        await _CategoryProductRepositoryAsync.AddRangeAsync(command.CategoryProducts);

                        await _productRepositoryAsync.CommitTransactionAsync();
                        return new Response<int>(product.Id);
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        await _productRepositoryAsync.RollbackTransactionAsync();
                        return new Response<int>(null);

                    }
                    catch (Exception ex)
                    {
                        throw new ApiException($"Lỗi");
                    }
                }
            }
        }
    }
}
