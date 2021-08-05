using Domain.Application.Command.OrderCommand;
using Domain.Application.Query.Order;
using Domain.Infrastructure.DTO.Request.Order;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(AddOrderDto addOrderDto)
        {
            var response = await _mediator.Send(new AddOrderCommand(addOrderDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }


        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var response = await _mediator.Send(new UpdateOrderCommand(updateOrderDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> GetOrder(string OrderNumber)
        {
            var response = await _mediator.Send(new GetOrderQuery(OrderNumber));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(Guid Uid,bool SoftDelete)
        {
            var response = await _mediator.Send(new DeleteOrderCommand(Uid,SoftDelete));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }




    }
}
