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
    public record OftenSoldProductsQuery(DateTime? Start, DateTime? End) : IRequest<ServiceResponse<List<OftenSoldProductsDto>>>;


    public class OftenSoldProductsHandler : IRequestHandler<OftenSoldProductsQuery, ServiceResponse<List<OftenSoldProductsDto>>>
    {
        private readonly IUnitOfWork<Consultant> _consultant;
        private readonly IUnitOfWork<OrderDetails> _orderDetails;
        private readonly IUnitOfWork<Model.Order> _orderofWork;

        public OftenSoldProductsHandler(IUnitOfWork<Consultant> Consultant, IUnitOfWork<OrderDetails> OrderDetails, IUnitOfWork<Domain.Model.Order> orderofWork)
        {
            _consultant = Consultant;
            _orderDetails = OrderDetails;
            _orderofWork = orderofWork;
        }
        public async Task<ServiceResponse<List<OftenSoldProductsDto>>> Handle(OftenSoldProductsQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<OftenSoldProductsDto>> serviceRespone = new();
            List<OftenSoldProductsDto> oftenSoldProductsDtos = new();


            var start = request.Start.HasValue ? request.Start.Value.Date : DateTime.MinValue.Date;
            var end = request.End.HasValue ? request.End.Value.Date : DateTime.MaxValue.Date;

            try
            {

                foreach (var item in _orderofWork.Repository.GetAll().Result.AsQueryable().Select(e => e.ConsultantsUid).Distinct().ToList())
                {
                    var data = _orderDetails.Repository.Where(e => !e.Deleted &&
                                        e.Orders.OrderDate >= start &&
                                        e.Orders.OrderDate <= end &&
                                        e.Orders.ConsultantsUid == item)
                                .Include(e => e.Products)
                                .Include(e => e.Orders)
                                .ThenInclude(e => e.Consultants)
                                .ToList();

                    var consultant = _consultant.Repository.GetById(item).Result;

                    var MostIncomeProduct = data.GroupBy(e => e.Products).Select(e => new ProductDto
                                { ProductCode = e.Key.Code, ProductName = e.Key.ProductName, CountSum = e.Sum(e => e.Products.Price) }).FirstOrDefault();


                    var OftenSoldProduct = data.GroupBy(e => e.Products).Select(e => new ProductDto
                                { ProductCode = e.Key.Code, ProductName = e.Key.ProductName, CountSum = e.Count() })
                                .OrderByDescending(e => e.CountSum)
                                .FirstOrDefault();

                    OftenSoldProductsDto oftenSoldProductsDto = new OftenSoldProductsDto()
                    {
                        MostIncomeProduct = MostIncomeProduct,
                        OftenSoldProduct = OftenSoldProduct,
                        ConsultantFirstName = consultant.FirstName,
                        ConsultantLastName = consultant.LastName,
                        ConsultantIdentification = consultant.Identification,
                        ConsultantUid = consultant.Uid
                    };

                    oftenSoldProductsDtos.Add(oftenSoldProductsDto);

                }



                serviceRespone.Data = oftenSoldProductsDtos;

                return serviceRespone;


            }
            catch (Exception ex)
            {
                serviceRespone.Message.Add(ex.Message);
                serviceRespone.Succes = false;
            }

            return serviceRespone;
        }
    }


}
