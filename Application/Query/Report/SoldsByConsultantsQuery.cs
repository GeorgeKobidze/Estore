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
    public record SoldsByConsultantsQuery(DateTime? Start, DateTime? End) : IRequest<ServiceResponse<List<SoldsByConsultantsDto>>>;


    public class SoldsByConsultantsHandler : IRequestHandler<SoldsByConsultantsQuery, ServiceResponse<List<SoldsByConsultantsDto>>>
    {
        private readonly IUnitOfWork<OrderDetails> _iunitofWork;
        private readonly IUnitOfWork<Consultant> _consultant;

        public SoldsByConsultantsHandler(IUnitOfWork<OrderDetails> iunitofWork,IUnitOfWork<Consultant> consultant)
        {
            _iunitofWork = iunitofWork;
            _consultant = consultant;
        }
        public async Task<ServiceResponse<List<SoldsByConsultantsDto>>> Handle(SoldsByConsultantsQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<SoldsByConsultantsDto>> serviceResponse = new();
            List<SoldsByConsultantsDto> solds = new(); 
            try
            {

                var start = request.Start.HasValue ? request.Start.Value.Date : DateTime.MinValue.Date;
                var end = request.End.HasValue ? request.End.Value.Date : DateTime.MaxValue.Date;

                foreach (var item in _consultant.Repository.Where(e=> !e.Deleted).ToList())
                {
                    decimal referalsum = 0;
                                  

                    if (_iunitofWork.Repository
                                   .Where(b => b.Orders.Consultants.ConsultantUid.ToString() == item.ToString()).Any())
                    {
                        referalsum = _iunitofWork.Repository
                                   .Where(b => b.Orders.Consultants.ConsultantUid.ToString() == item.ToString())
                                    .Include(b => b.Orders)
                                   .ThenInclude(b => b.Consultants)
                                   .ThenInclude(b => b.Recomendator)
                                   .Sum(e => e.Orders.Sum);
                    }


                    solds.AddRange( _iunitofWork.Repository.Where(e => !e.Deleted && e.Orders.OrderDate >= start && e.Orders.OrderDate <= end)
                                    .Include(e => e.Orders)
                                    .ThenInclude(e => e.Consultants)
                                    .Select(e => new SoldsByConsultantsDto
                                    {
                                        ConsultantFirstName = e.Orders.Consultants.FirstName,
                                        ConsultantIdentification = e.Orders.Consultants.Identification,
                                        ConsultantLastName = e.Orders.Consultants.LastName,
                                        ConsultantUid = e.Orders.Consultants.Uid,
                                        OrderSums = e.Orders.Consultants.Orders.Sum(e => e.Sum),
                                        RefelralSums = referalsum

                                    }).ToList());

                }

                serviceResponse.Data = solds;

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
