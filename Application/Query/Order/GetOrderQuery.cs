using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Response.Order;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Infrastructure.DTO.Response.Consultant;
using Domain.Infrastructure.DTO.Response.Product;

namespace Domain.Application.Query.Order
{
    public record GetOrderQuery(string OrderNumber) : IRequest<ServiceResponse<GetOrderDto>>;


    public class GetOrderHandler : IRequestHandler<GetOrderQuery, ServiceResponse<GetOrderDto>>
    {
        private readonly IUnitOfWork<OrderDetails> _iunitofWork;
        private readonly IUnitOfWork<Product> _produnitOfWork;
        private readonly IMapper _mapper;

        public GetOrderHandler(IUnitOfWork<OrderDetails> iunitofWork,IUnitOfWork<Product> produnitOfWork,IMapper mapper)
        {
            _iunitofWork = iunitofWork;
            _produnitOfWork = produnitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetOrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<GetOrderDto> serviceResponse = new();

            try
            {
                List<Product> products = new List<Product>();

                foreach (var item in
                 _iunitofWork.Repository.Where(e => e.Orders.OrderNumber == request.OrderNumber && !e.Orders.Deleted)
                                .Select(e => e.ProductsUid))
                {
                    products.Add(_produnitOfWork.Repository.GetById(item).Result);
                }

                serviceResponse.Data = _iunitofWork.Repository.Where(e => e.Orders.OrderNumber == request.OrderNumber && !e.Orders.Deleted)
                                .Include(e=> e.Products)
                                .Include(e => e.Orders)                                
                                .ThenInclude(e => e.Consultants)                                
                                .Select(e => new GetOrderDto 
                                {                                
                                    Consultant = _mapper.Map<ConsultantDto>(e.Orders.Consultants),
                                    OrderDate = e.Orders.OrderDate,
                                    OrderNumber = e.Orders.OrderNumber,
                                    ProductCount = e.Orders.ProductCount,
                                    Sum = e.Orders.Sum,
                                    Products = _mapper.Map<List<GetProductDto>>(products)  
                                })
                                .FirstOrDefault();
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
