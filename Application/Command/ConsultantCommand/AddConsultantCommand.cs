using Domain.Infrastructure.DTO.Consultant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain.Infrastructure.DTO.Common;
using System.Threading;
using Domain.Infrastructure.Interface;
using Domain.Model;
using AutoMapper;
using Domain.Application.Helpers.Cosnultant;

namespace Domain.Application.Command.ConsultantCommand
{
    public record AddConsultantCommand(AddConsultantDto AddConsultantDto) : IRequest<ServiceResponse<string>>;




    public class AddConsultantHandler : IRequestHandler<AddConsultantCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Consultant> _consultant;
        private readonly IMapper _mapper;

        public AddConsultantHandler(IUnitOfWork<Consultant> Consultant, IMapper mapper)
        {
            _consultant = Consultant;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Handle(AddConsultantCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {
                var consultant = _mapper.Map<Consultant>(request.AddConsultantDto);
                serviceResponse = new ValidateConsultant(consultant, _consultant).Validate();

                if (serviceResponse.Succes)
                {
                    await _consultant.Repository.Add(consultant);
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
