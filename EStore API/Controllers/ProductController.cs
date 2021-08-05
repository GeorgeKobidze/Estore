using Domain.Application.Command.ProductCommand;
using Domain.Application.Query.ProductQuery;
using Domain.Infrastructure.DTO.Request.Product;
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
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            var response = await _mediator.Send(new AddProductCommand(addProductDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var response = await _mediator.Send(new UpdateProductCommand(updateProductDto));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }



        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(Guid Uid)
        {
            var response = await _mediator.Send(new GetProductQuery(Uid));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid Uid,bool SoftDelete)
        {
            var response = await _mediator.Send(new DeleteProductCommand(Uid,SoftDelete));

            if (!response.Succes)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

    }
}
