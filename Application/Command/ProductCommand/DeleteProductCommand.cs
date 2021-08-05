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

namespace Domain.Application.Command.ProductCommand
{
    public record DeleteProductCommand(Guid Uid,bool SoftDelete) : IRequest<ServiceResponse<string>>;



    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Product> _iunitofWork;
        private readonly IUnitOfWork<OrderDetails> _orderdeunitOfWork;

        public DeleteProductHandler(IUnitOfWork<Product> iunitofWork,IUnitOfWork<OrderDetails> OrderdeunitOfWork)
        {
            _iunitofWork = iunitofWork;
            _orderdeunitOfWork = OrderdeunitOfWork;
        }
        public async Task<ServiceResponse<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {
                var product = _iunitofWork.Repository.Where(e => e.Uid == request.Uid).FirstOrDefault();

                if (product ==null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("product is invalid");
                    return serviceResponse;
                }

                if (!request.SoftDelete)
                {
                    _iunitofWork.Repository.Delete(product);
                    await _iunitofWork.CommitAsync();
                    serviceResponse.Data = "Product Deleted";
                    return serviceResponse;
                }

                var orderdet = _orderdeunitOfWork.Repository.Where(e => e.ProductsUid == request.Uid).ToList();

                foreach (var item in orderdet)
                {
                    item.Deleted = true;
                    _orderdeunitOfWork.Repository.Update(item);                    
                }
                product.Deleted = true;
                _iunitofWork.Repository.Update(product);
                await _iunitofWork.CommitAsync();
                serviceResponse.Data = "Product Deleted";
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
