using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Commands.CreateProduct;
using VietCapital.Partner.F5Seconds.Application.Features.Products.Queries.GetAllProducts;
using VietCapital.Partner.F5Seconds.Domain.Entities;

namespace VietCapital.Partner.F5Seconds.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
