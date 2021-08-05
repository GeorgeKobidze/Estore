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
    public record ConsultantsWithOftenSoldProductsQuery(DateTime Start, DateTime End, int Count, string ProductCode = null) : IRequest<ServiceResponse<List<ConsultantsWithOftenSoldProductsDto>>>;



    public class ConsultantsWithOftenSoldProductsHandler : IRequestHandler<ConsultantsWithOftenSoldProductsQuery, ServiceResponse<List<ConsultantsWithOftenSoldProductsDto>>>
    {
        private readonly IUnitOfWork<OrderDetails> _iunitOfwork;

        public ConsultantsWithOftenSoldProductsHandler(IUnitOfWork<OrderDetails> iunitOfwork)
        {
            _iunitOfwork = iunitOfwork;
        }
        public async Task<ServiceResponse<List<ConsultantsWithOftenSoldProductsDto>>> Handle(ConsultantsWithOftenSoldProductsQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<ConsultantsWithOftenSoldProductsDto>> serviceResponse = new();

            try
            {
                var data = _iunitOfwork.Repository.Where(e => !e.Deleted && e.Orders.OrderDate.Date >= request.Start.Date && e.Orders.OrderDate.Date <= request.End.Date)
                    .Include(e => e.Products)
                    .Include(e => e.Orders)
                    .ThenInclude(e => e.Consultants)
                    .Select(e => new ConsultantsWithOftenSoldProductsDto
                    {
                        ConsultantFirstName = e.Orders.Consultants.FirstName,
                        ConsultantIdentification = e.Orders.Consultants.Identification,
                        ConsultantLastName = e.Orders.Consultants.LastName,
                        ConsultantUid = e.Orders.Consultants.Uid,
                        ProductCode = e.Products.Code,
                        ProductCount = e.Products.Code.Count()
                    })                   
                    .ToList();

                serviceResponse.Data = data.Where(e => e.ProductCount >= request.Count).ToList();


                if(request.ProductCode != null)
                {
                    serviceResponse.Data = serviceResponse.Data.Where(e => e.ProductCode == request.ProductCode).ToList();
                    return serviceResponse;
                }
                return serviceResponse;                

            }
            catch(Exception ex)
            {
                serviceResponse.Message.Add(ex.Message);
                serviceResponse.Succes = false;
            }
            return serviceResponse;


        }
    }

}
