using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Infrastructure.DTO.Response.Product;
using Domain.Infrastructure.DTO.Common;
using System.Threading;
using Domain.Model;
using Domain.Infrastructure.Interface;

namespace Domain.Application.Query.ProductQuery
{
    public record GetProductQuery(Guid Uid) : IRequest<ServiceResponse<GetProductDto>>;


    public class GetProductHandler : IRequestHandler<GetProductQuery, ServiceResponse<GetProductDto>>
    {
        private readonly IUnitOfWork<Product> _iunitofwork;
        private readonly IMapper _mapper;

        public GetProductHandler(IUnitOfWork<Product> iunitofwork,IMapper mapper)
        {
            _iunitofwork = iunitofwork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<GetProductDto> serviceResponse = new();

            try
            {
                var product = _iunitofwork.Repository.Where(e => e.Uid == request.Uid && !e.Deleted).FirstOrDefault();

                if (product == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add( "Product doesnot exciting");
                    return serviceResponse;
                }
                serviceResponse.Data = _mapper.Map<GetProductDto>(product);
            }
            catch (Exception ex)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add(ex.Message);
            }
            return serviceResponse;

        }
    }


}
