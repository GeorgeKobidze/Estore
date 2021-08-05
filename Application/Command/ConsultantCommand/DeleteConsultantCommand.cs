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

namespace Domain.Application.Command.ConsultantCommand
{
    public record DeleteConsultantCommand(Guid Uid,bool SoftDelete) : IRequest<ServiceResponse<string>>;



    public class DeleteConsultantHandler : IRequestHandler<DeleteConsultantCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Consultant> _iunitofWork;
        private readonly IUnitOfWork<Order> _orderunitOfWork;

        public DeleteConsultantHandler(IUnitOfWork<Consultant> iunitofWork,IUnitOfWork<Order> orderunitOfWork)
        {
            _iunitofWork = iunitofWork;
            _orderunitOfWork = orderunitOfWork;
        }

        public async Task<ServiceResponse<string>> Handle(DeleteConsultantCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {
                var consultant = _iunitofWork.Repository.GetById(request.Uid).Result;

                if (consultant == null)
                {

                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("consultant is invalid");
                    return serviceResponse;
                }

                if (!request.SoftDelete)
                {
                    _iunitofWork.Repository.Delete(consultant);
                    await _iunitofWork.CommitAsync();

                    serviceResponse.Data = "Deleted succ";
                    return serviceResponse;
                }

                var order = _orderunitOfWork.Repository.Where(e => e.Consultants == consultant).ToList();

                foreach (var item in order)
                {
                    item.Deleted = true;
                    _orderunitOfWork.Repository.Update(item);
                }


                consultant.Deleted = true;
                _iunitofWork.Repository.Update(consultant);
                await _iunitofWork.CommitAsync();

                serviceResponse.Data = "Deleted succ";
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
