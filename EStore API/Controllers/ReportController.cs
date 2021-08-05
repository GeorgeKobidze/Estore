using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Application.Query.Report;

namespace EStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("OrderByConsultants")]
        public async Task<IActionResult> OrderByConsultants(DateTime Start,DateTime End)
        {
            var response = await _mediator.Send(new OrderByConsultantsQuery(Start, End));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet("OrderByProducts")]
        public async Task<IActionResult> OrderByProducts(DateTime Start, DateTime End,decimal MinPrice,decimal MaxPrice)
        {
            var response = await _mediator.Send(new OrderByProductsQuery(Start, End,MinPrice,MaxPrice));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet("ConsultantsWithOftenSoldProducts")]
        public async Task<IActionResult> ConsultantsWithOftenSoldProducts(DateTime Start, DateTime End,int Count,string ProductCode = null)
        {
            var response = await _mediator.Send(new ConsultantsWithOftenSoldProductsQuery(Start, End,Count,ProductCode));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("SoldsByConsultants")]
        public async Task<IActionResult> SoldsByConsultants(DateTime? Start , DateTime? End)
        {
            var response = await _mediator.Send(new SoldsByConsultantsQuery(Start, End));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpGet("OftenSoldProducts")]
        public async Task<IActionResult> OftenSoldProducts(DateTime? Start, DateTime? End)
        {
            var response = await _mediator.Send(new OftenSoldProductsQuery(Start, End));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }




    }
}
