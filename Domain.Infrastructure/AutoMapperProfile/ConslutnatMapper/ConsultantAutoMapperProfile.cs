using AutoMapper;
using Domain.Infrastructure.DTO.Consultant;
using Domain.Infrastructure.DTO.Request.Consultant;
using Domain.Infrastructure.DTO.Response.Consultant;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.AutoMapperProfile.ConslutnatMapper
{
    public class ConsultantAutoMapperProfile : Profile
    {
        public ConsultantAutoMapperProfile()
        {
            CreateMap<AddConsultantDto, Consultant>()
             .ForMember(c => c.LastModifiedDateTime, op => op.Ignore())
             .ForMember(c => c.CreatedDateTime, op => op.Ignore())
             .ForMember(c => c.Deleted, op => op.Ignore())
             .ForMember(c => c.DateOfBirth, op => op.MapFrom(src => Convert.ToDateTime(src.DateOfBirth)));

            CreateMap<UpdateConsultantDto, Consultant>()
             .ForMember(c => c.LastModifiedDateTime, op => op.Ignore())
             .ForMember(c => c.CreatedDateTime, op => op.Ignore())
             .ForMember(c => c.Deleted, op => op.Ignore())
             .ForMember(c => c.DateOfBirth, op => op.MapFrom(src => Convert.ToDateTime(src.DateOfBirth)))
             .ForAllMembers(c => c.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Consultant, ConsultantDto>()
             .ForSourceMember(c => c.LastModifiedDateTime, op => op.DoNotValidate())
             .ForSourceMember(c => c.CreatedDateTime, op => op.DoNotValidate())
             .ForSourceMember(c => c.Deleted, op => op.DoNotValidate())
             .ForSourceMember(c => c.ConsultantUid, op => op.DoNotValidate())
             .ForSourceMember(c => c.Orders, op => op.DoNotValidate())
             .ForSourceMember(c => c.Recomendator, op => op.DoNotValidate());


        }
    }
}
