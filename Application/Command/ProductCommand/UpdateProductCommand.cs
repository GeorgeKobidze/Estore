using Domain.Infrastructure.DTO.Request.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Domain.Infrastructure.DTO.Common;
using System.Threading;
using Domain.Model;
using Domain.Infrastructure.Interface;
using AutoMapper;

namespace Domain.Application.Command.ProductCommand
{
    public record UpdateProductCommand(UpdateProductDto UpdateProductDto) : IRequest<ServiceResponse<string>>;
    
    
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand,ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Product> _iunitofWork;
        private readonly IMapper _mapper;

        public UpdateProductHandler(IUnitOfWork<Product> iunitofWork,IMapper mapper)
        {
            _iunitofWork = iunitofWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {

                var _product = _iunitofWork.Repository.Where(e => e.Uid == request.UpdateProductDto.Uid).FirstOrDefault();             


                if (_product == null)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Product does not existing");
                    return serviceResponse;
                }
                var product = _mapper.Map<UpdateProductDto, Product>(request.UpdateProductDto, _product);

                if (_iunitofWork.Repository.Where(e => e.Code == product.Code && e.Uid != product.Uid).Any())
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Product with code already existing");
                }
                if (product.Price < 1)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("Price must be higher than 1");
                }
                if (product.ProductName.Length < 4)
                {
                    serviceResponse.Succes = false;
                    serviceResponse.Message.Add("ProductName Length must be greater than 4");
                }


                if (serviceResponse.Succes)
                {
                    _iunitofWork.Repository.Update(product);
                    await _iunitofWork.CommitAsync();
                    serviceResponse.Data = "Product updated";
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
