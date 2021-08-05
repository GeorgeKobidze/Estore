using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Infrastructure.DTO.Common;
using Domain.Infrastructure.DTO.Response.Consultant;
using Domain.Infrastructure.Interface;
using Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Domain.Application.Query.ConsultantQuery
{
    public record GetConsultantQuery(Guid Uid) : IRequest<ServiceResponse<GetConsultantDto>>;



    public class GetConsultantHandler : IRequestHandler<GetConsultantQuery, ServiceResponse<GetConsultantDto>>
    {

        private readonly IUnitOfWork<Consultant> _consultant;
        private readonly IMapper _mapper;

        public GetConsultantHandler(IUnitOfWork<Consultant> Consultant,IMapper mapper)
        {
            _consultant = Consultant;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<GetConsultantDto>> Handle(GetConsultantQuery request, CancellationToken cancellationToken)
        {
            ServiceResponse<GetConsultantDto> serviceResponse = new();
            try
            {

                var recemndationGiven = _mapper.Map<List<ConsultantDto>>(_consultant.Repository.Where(e => e.ConsultantUid == request.Uid).ToList());

                var Recommendation = _mapper.Map<ConsultantDto>(_consultant.Repository
                                        .Where(e => e.Uid == request.Uid && e.Recomendator != null).FirstOrDefault());
                

                var consultant = _mapper.Map<ConsultantDto>(_consultant.Repository.Where(e => e.Uid == request.Uid).FirstOrDefault());

                serviceResponse.Data = new GetConsultantDto()
                {
                    consultant = consultant,
                    Recommendation = Recommendation,
                    RecommendationsGiven = recemndationGiven
                };                                      
                                        

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
