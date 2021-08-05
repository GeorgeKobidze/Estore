using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;

namespace Domain.Application.Command.OrderCommand
{
    public record DeleteOrderCommand(Guid Uid,bool SoftDelete) : IRequest<ServiceResponse<string>>;



    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Order> _iunitofWork;
        private readonly IUnitOfWork<OrderDetails> _orderDetailsunitOfWork;

        public DeleteOrderHandler(IUnitOfWork<Order> iunitofWork,IUnitOfWork<OrderDetails> OrderDetailsunitOfWork)
        {
            _iunitofWork = iunitofWork;
            _orderDetailsunitOfWork = OrderDetailsunitOfWork;
        }

        public async Task<ServiceResponse<string>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();

            try
            {
                var order = _iunitofWork.Repository.GetById(request.Uid).Result;

                if (order == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("order is invalid");
                    return serviceResponse;
                }

                if (!request.SoftDelete)
                {   
                    _iunitofWork.Repository.Delete(order);
                    await _iunitofWork.CommitAsync();
                    serviceResponse.Data = "Order Deleted";
                    return serviceResponse;
                }

                var orderdet = _orderDetailsunitOfWork.Repository.Where(e => e.Orders == order).ToList();

                foreach (var item in orderdet)
                {
                    item.Deleted = true;
                    _orderDetailsunitOfWork.Repository.Update(item);
                }

                order.Deleted = true;
                _iunitofWork.Repository.Update(order);

                await _iunitofWork.CommitAsync();
                serviceResponse.Data = "Order Deleted";
                return serviceResponse;


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
