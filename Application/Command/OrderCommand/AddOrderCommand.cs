using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Request.Order;
using System.Threading;
using Domain.Model;
using Domain.Infrastructure.Interface;
using Domain.Application.Helpers.Orders;

namespace Domain.Application.Command.OrderCommand
{
    public record AddOrderCommand(AddOrderDto AddOrderDto) : IRequest<ServiceResponse<string>>;


    public class AddOrderHandler : IRequestHandler<AddOrderCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Order> _orderIunitofwork;
        private readonly IUnitOfWork<Product> _productIunitofwork;
        private readonly IUnitOfWork<OrderDetails> _orderDetailsunitOfWork;
        private readonly IUnitOfWork<Consultant> _consultantunitOfWork;

        public AddOrderHandler(IUnitOfWork<Order> OrderIunitofwork,IUnitOfWork<Product> ProductIunitofwork,
                        IUnitOfWork<OrderDetails> OrderDetailsunitOfWork,IUnitOfWork<Consultant> ConsultantunitOfWork)
        {
            _orderIunitofwork = OrderIunitofwork;
            _productIunitofwork = ProductIunitofwork;
            _orderDetailsunitOfWork = OrderDetailsunitOfWork;
            _consultantunitOfWork = ConsultantunitOfWork;
        }
        public async Task<ServiceResponse<string>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();

            try
            {
                var consultant = _consultantunitOfWork.Repository.Where(e => e.Uid == request.AddOrderDto.ConsultantsUid && !e.Deleted).FirstOrDefault();

                List<Product> products = new List<Product>();

                if (consultant == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Invalid Consultant");
                    return serviceResponse;
                }

                foreach (var uid in request.AddOrderDto.Products)
                {
                    var prodcut = _productIunitofwork.Repository.Where(e => e.Uid == uid.Uid && !e.Deleted).FirstOrDefault();
                    if (prodcut == null)
                    {
                        serviceResponse.Succes = false;
                        serviceResponse.Message.Add("Invalid Product");
                        return serviceResponse;
                    }
                    products.Add(prodcut);
                }

                Order order = new Order()
                {
                    ConsultantsUid = request.AddOrderDto.ConsultantsUid,
                    OrderDate = DateTime.Now,
                    ProductCount = products.Count,
                    Sum = products.Sum(e => e.Price),
                    OrderNumber = new GenarateOrderNumber(_orderIunitofwork).GenerateUniqueOrdernumber()
                };

                await _orderIunitofwork.Repository.Add(order);

                foreach (var item in products)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        Orders = order,
                        ProductsUid = item.Uid
                    };
                   await _orderDetailsunitOfWork.Repository.Add(orderDetails);

                   serviceResponse.Data = "Order Added";
                }

                await _orderIunitofwork.CommitAsync();


            }
            catch(Exception ex)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message.Add(ex.Message);
            }
            return serviceResponse;
        }
    }


}
