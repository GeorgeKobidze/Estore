using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Response.Report;
using Domain.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Application.Query.Report
{
    public record OrderByProductsQuery(DateTime Start, DateTime End, decimal MinPrice, decimal MaxPrice) : IRequest<ServiceResponse<List<OrderByConsultantsDto>>>;




    public class OrderByProductsHandler : IRequestHandler<OrderByProductsQuery, ServiceResponse<List<OrderByConsultantsDto>>>
    {
        private readonly IUnitOfWork<Model.OrderDetails> _iunitofWork;

        public OrderByProductsHandler(IUnitOfWork<Domain.Model.OrderDetails> iunitofWork)
        {
            _iunitofWork = iunitofWork;
        }
        public async Task<ServiceResponse<List<OrderByConsultantsDto>>> Handle(OrderByProductsQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<OrderByConsultantsDto>> serviceResponse = new();
            try
            {
                serviceResponse.Data = _iunitofWork.Repository
                                .Where(e => e.Orders.OrderDate.Date >= request.Start.Date && e.Orders.OrderDate.Date <= request.End.Date 
                                && e.Products.Price >= request.MinPrice && e.Products.Price <= request.MaxPrice
                                && !e.Orders.Deleted)
                                .Include(e => e.Products)
                                .Include(e => e.Orders)
                                .ThenInclude(e => e.Consultants)
                                .ToList().Select(e =>
                               new OrderByConsultantsDto
                               {
                                   ConsultantFirstName = e.Orders.Consultants.FirstName,
                                   ConsultantIdentification = e.Orders.Consultants.Identification,
                                   ConsultantLastName = e.Orders.Consultants.LastName,
                                   ConsultantUid = e.Orders.Consultants.Uid,
                                   OrderDate = e.Orders.OrderDate,
                                   OrderNumber = e.Orders.OrderNumber,
                                   OrderSum = e.Orders.Sum,
                                   OrderUid = e.Orders.Uid,
                                   ProductCount = e.Orders.ProductCount
                               }).ToList();
                serviceResponse.Succes = true;
                return serviceResponse;

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
