using AutoMapper;
using Domain.Infrastructure.DTO.Request.Product;
using Domain.Infrastructure.DTO.Response.Product;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.AutoMapperProfile.ProductMapper
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            
            CreateMap<AddProductDto, Product>()
             .ForMember(c => c.LastModifiedDateTime, op => op.Ignore())
             .ForMember(c => c.CreatedDateTime, op => op.Ignore())
             .ForMember(c => c.Deleted, op => op.Ignore());


            CreateMap<UpdateProductDto, Product>()
             .ForMember(c => c.LastModifiedDateTime, op => op.Ignore())
             .ForMember(c => c.CreatedDateTime, op => op.Ignore())
             .ForMember(c => c.Deleted, op => op.Ignore())
             .ForAllMembers(c => c.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Product, GetProductDto>()
            .ForSourceMember(c => c.LastModifiedDateTime, op => op.DoNotValidate())
            .ForSourceMember(c => c.CreatedDateTime, op => op.DoNotValidate())
            .ForSourceMember(c => c.Deleted, op => op.DoNotValidate());
        }
    }
}
