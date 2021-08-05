using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Request.Order;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;

namespace Domain.Application.Command.OrderCommand
{
    public record UpdateOrderCommand(UpdateOrderDto UpdateOrderDto) : IRequest<ServiceResponse<string>>;



    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, ServiceResponse<string>>
    {

        private readonly IUnitOfWork<Order> _orderIunitofwork;
        private readonly IUnitOfWork<Product> _productIunitofwork;
        private readonly IUnitOfWork<OrderDetails> _orderDetailsunitOfWork;
        private readonly IUnitOfWork<Consultant> _consultantunitOfWork;

        public UpdateOrderHandler(IUnitOfWork<Order> OrderIunitofwork, IUnitOfWork<Product> ProductIunitofwork,
                        IUnitOfWork<OrderDetails> OrderDetailsunitOfWork, IUnitOfWork<Consultant> ConsultantunitOfWork)
        {
            _orderIunitofwork = OrderIunitofwork;
            _productIunitofwork = ProductIunitofwork;
            _orderDetailsunitOfWork = OrderDetailsunitOfWork;
            _consultantunitOfWork = ConsultantunitOfWork;
        }

        public async Task<ServiceResponse<string>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {
                var order = _orderIunitofwork.Repository.Where(e=> e.OrderNumber == request.UpdateOrderDto.OrderNumber && !e.Deleted).FirstOrDefault();
                Consultant consultant;

                if (order == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Order is not valid");
                    return serviceResponse;
                }

                if (request.UpdateOrderDto.Consultant != Guid.Empty)
                {
                    consultant = _consultantunitOfWork.Repository.Where(e => e.Uid == request.UpdateOrderDto.Consultant && !e.Deleted).FirstOrDefault();

                    if (consultant == null)
                    {
                        serviceResponse.Succes = false;
                        serviceResponse.Message.Add("consultant is not valid");
                        return serviceResponse;
                    }

                    order.Consultants = consultant;
                    _orderIunitofwork.Repository.Update(order);
                   await _orderIunitofwork.CommitAsync();

                }

                if (request.UpdateOrderDto.NewProducts != null)
                {
                    List<Product> prods = new List<Product>();
                    foreach (var newprod in request.UpdateOrderDto.NewProducts)
                    {
                        var prod = _productIunitofwork.Repository.Where(e => e.Uid == newprod.Uid && !e.Deleted).FirstOrDefault();
                        if (prod != null)
                        {
                            prods.Add(prod);
                        }
                    }

                    if (prods.Count >= 1)
                    {
                        foreach (var item in prods)
                        {

                            var odet = new OrderDetails()
                            {
                                Orders = order,
                                ProductsUid = item.Uid
                            };
                            await _orderDetailsunitOfWork.Repository.Add(odet);
                            await _orderDetailsunitOfWork.CommitAsync();
                        }
                    }

                }

                if (request.UpdateOrderDto.ProductsForDeletion != null)
                {
                    foreach (var oldprod in request.UpdateOrderDto.ProductsForDeletion)
                    {
                        if (_productIunitofwork.Repository.Where(e => e.Uid == oldprod.Uid && !e.Deleted).FirstOrDefault() != null)
                        {
                            _orderDetailsunitOfWork.Repository.Delete(_orderDetailsunitOfWork.Repository.Where(e => e.Orders == order).FirstOrDefault());
                            await _orderDetailsunitOfWork.CommitAsync();
                        }
                    }
                }

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
