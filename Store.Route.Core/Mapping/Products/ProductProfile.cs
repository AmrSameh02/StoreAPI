﻿using AutoMapper;
using Store.Route.Core.DTOs.Products;
using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName, options => options.MapFrom(d => d.Brand.Name))
                .ForMember(d => d.TypeName, options => options.MapFrom(d => d.Type.Name));

            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}