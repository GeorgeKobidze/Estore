using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Response.Report;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Application.Query.Report
{
    public record OrderByConsultantsQuery(DateTime Start,DateTime End) : IRequest<ServiceResponse<List<OrderByConsultantsDto>>>;


    public class OrderByConsultantsHandler : IRequestHandler<OrderByConsultantsQuery, ServiceResponse<List<OrderByConsultantsDto>>>
    {
        private readonly IUnitOfWork<Model.Order> _iunitofWork;

        public OrderByConsultantsHandler(IUnitOfWork<Domain.Model.Order> iunitofWork)
        {
            _iunitofWork = iunitofWork;
        }
        public async Task<ServiceResponse<List<OrderByConsultantsDto>>> Handle(OrderByConsultantsQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<OrderByConsultantsDto>> serviceResponse = new();

            try
            {
                serviceResponse.Data = _iunitofWork.Repository.Where(e => e.OrderDate.Date >= request.Start.Date && e.OrderDate.Date <= request.End.Date && !e.Deleted)
                                .Include(e => e.Consultants).ToList().Select( e=>  
                                new OrderByConsultantsDto {
                                    ConsultantFirstName = e.Consultants.FirstName,
                                    ConsultantIdentification = e.Consultants.Identification,
                                    ConsultantLastName = e.Consultants.LastName,
                                    ConsultantUid = e.Consultants.Uid,
                                    OrderDate = e.OrderDate,
                                    OrderNumber = e.OrderNumber,
                                    OrderSum = e.Sum,
                                    OrderUid = e.Uid,
                                    ProductCount = e.ProductCount 
                                }).ToList();
                serviceResponse.Succes = true;
                return serviceResponse;

            }
            catch (Exception ex)
            {
                serviceResponse.Message.Add(ex.Message);
                serviceResponse.Succes = false;
            }
            return serviceResponse;

        }
    }

}
