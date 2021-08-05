using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Application.Helpers.Cosnultant;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Request.Consultant;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;

namespace Domain.Application.Command.ConsultantCommand
{
    public record UpdateConsultantCommand(UpdateConsultantDto UpdateConsultantDto) : IRequest<ServiceResponse<string>>;



    public class UpdateConsultantHandler : IRequestHandler<UpdateConsultantCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Consultant> _consultant;
        private readonly IMapper _mapper;

        public UpdateConsultantHandler(IUnitOfWork<Consultant> Consultant, IMapper mapper)
        {
            _consultant = Consultant;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> Handle(UpdateConsultantCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();

            try
            {

                var excconsultant = await _consultant.Repository.GetById(request.UpdateConsultantDto.Uid);

                if (excconsultant == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Consultant is not in DB");
                }

                var consultant = _mapper.Map<UpdateConsultantDto,Consultant>(request.UpdateConsultantDto,excconsultant);

                consultant.DateOfBirth = excconsultant.DateOfBirth;

                serviceResponse = new ValidateConsultant(consultant, _consultant).Validate();

                if (request.UpdateConsultantDto.ConsultantUid == consultant.Uid)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("You can't recommendation yourself");
                }

                if (serviceResponse.Succes)
                {
                    _consultant.Repository.Update(consultant);
                    await _consultant.CommitAsync();
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
