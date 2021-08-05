using AutoMapper;
using Domain.Infrastructure.AutoMapperProfile.ConslutnatMapper;
using Domain.Infrastructure.AutoMapperProfile.ProductMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.AutoMapperProfile
{
    public class AutoMappersProfile : Profile
    {

        public AutoMappersProfile()
        {
            new ConsultantAutoMapperProfile();
            new ProductAutoMapperProfile();
        }
    }
}
