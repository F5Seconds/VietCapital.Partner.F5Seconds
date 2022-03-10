using AutoMapper;
using System;
using System.Globalization;
using System.Linq;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
using VietCapital.Partner.F5Seconds.Application.Features.Categories.Queries.ListCategory;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.ListProduct;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Commands;
using VietCapital.Partner.F5Seconds.Application.Features.Transactions.Queries.GetVoucherTransFilter;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<CreateTransactionCommand, BuyVoucherPayload>();
            CreateMap<F5sVoucherCode, VoucherTransaction>()
                .ForMember(d => d.ProductPrice, m => m.MapFrom(s => s.productPrice))
                .ForMember(d => d.TransactionId, m => m.MapFrom(s => s.transactionId))
                .ForMember(d => d.VoucherCode, m => m.MapFrom(s => s.voucherCode))
                .ForMember(d => d.ExpiryDate, m => m.MapFrom(s => DateTime.ParseExact(s.expiryDate, "yyyy-MM-dd", CultureInfo.CurrentCulture)));

            #region Product
            CreateMap<GetListProductQuery, GetListProductParameter>();
            CreateMap<Product, ProductOutSideResponse>()
                .ForMember(d => d.Categories, m => m.MapFrom(s => s.CategoryProducts.Select(c => new CategoryInsideResponse()
                {
                    Id = c.Category.Id,
                    Name = c.Category.Name,
                    Image = c.Category.Image,
                    Status = c.Category.Status
                })));
            
            #endregion

            #region Transaction
            CreateMap<GetVoucherTransFilterQuery, GetVoucherTransFilterParameter>();
            CreateMap<VoucherTransaction, TransactionOutSideResponse>()
                .ForMember(d => d.Product, m => m.MapFrom(s => new ProductInSideResponse()
                {
                    Id = s.Product.Id,
                    BrandLogo = s.Product.BrandLogo,
                    BrandName = s.Product.BrandName,
                    Content = s.Product.Content,
                    Image = s.Product.Image,
                    Name = s.Product.Name,
                    Partner = s.Product.Partner,
                    Point = s.Product.Point,
                    ProductCode = s.Product.ProductCode,
                    Term = s.Product.Term,
                    Thumbnail = s.Product.Thumbnail,
                    Type = s.Product.Type
                }));
            #endregion

            #region Category
            CreateMap<GetListCategoryQuery, GetListCategoryParameter>();
            CreateMap<Category, CategoryOutsideResponse>()
                .ForMember(d => d.Products, m => m.MapFrom(s => s.CategoryProducts.Select(p => new ProductInSideResponse()
                {
                    Id = p.Product.Id,
                    BrandLogo = p.Product.BrandLogo,
                    BrandName = p.Product.BrandName,
                    Content = p.Product.Content,
                    Image = p.Product.Image,
                    Name = p.Product.Name,
                    Partner = p.Product.Partner,
                    Point = p.Product.Point,
                    ProductCode = p.Product.ProductCode,
                    Term = p.Product.Term,
                    Thumbnail = p.Product.Thumbnail,
                    Type = p.Product.Type
                })));
            #endregion
        }
    }
}
