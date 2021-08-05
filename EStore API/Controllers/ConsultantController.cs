using Domain.Infrastructure.DTO.Consultant;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Application.Command.ConsultantCommand;
using Domain.Infrastructure.DTO.Request.Consultant;
using Domain.Application.Query.ConsultantQuery;

namespace EStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConsultantController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("AddConsultant")]
        public async Task<IActionResult> AddConsultant(AddConsultantDto addConsultantDto)
        {
            var response = await _mediator.Send(new AddConsultantCommand(addConsultantDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpPost("UpdateConsultant")]
        public async Task<IActionResult> UpdateConsultant(UpdateConsultantDto updateConsultantDto)
        {
            var response = await _mediator.Send(new UpdateConsultantCommand(updateConsultantDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet("GetConsultant")]
        public async Task<IActionResult> GetConsultant(Guid Uid)
        {
            var response = await _mediator.Send(new GetConsultantQuery(Uid));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpDelete("DeleteConsultant")]
        public async Task<IActionResult> DeleteConsultant(Guid Uid,bool SoftDelete)
        {
            var response = await _mediator.Send(new DeleteConsultantCommand(Uid,SoftDelete));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }
    }
}
