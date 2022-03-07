using AutoMapper;
using System;
using System.Globalization;
using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
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
            #endregion

            #region Transaction
            CreateMap<GetVoucherTransFilterQuery, GetVoucherTransFilterParameter>();
            #endregion
        }
    }
}
