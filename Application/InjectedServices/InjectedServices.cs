using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain.Infrastructure.Interface;
using Domain.Infrastructure.NewFolder;
using Domain.Infrastructure.AutoMapperProfile;

namespace Domain.Application.InjectedServices
{
    public static class InjectedServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<EStoreDBContext>(options => options.UseSqlServer(ConnectionString));

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            
            services.AddAutoMapper(typeof(AutoMappersProfile));
            
            services.AddMediatR(typeof(InjectedServices));
            return services;
        }

    }
}
