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
    public record AddProductCommand(AddProductDto AddProductDto) : IRequest<ServiceResponse<string>>;


    public class AddProductHanlder : IRequestHandler<AddProductCommand, ServiceResponse<string>>
    {
        private readonly IUnitOfWork<Product> _productWork;
        private readonly IMapper _mapper;

        public AddProductHanlder(IUnitOfWork<Product> ProductWork,IMapper mapper)
        {
            _productWork = ProductWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            ServiceResponse<string> serviceResponse = new();
            try
            {
                var product = _mapper.Map<Product>(request.AddProductDto);

                if (_productWork.Repository.Where(e => e.Code == product.Code).Any())
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
                    await _productWork.Repository.Add(product);
                    await _productWork.CommitAsync();
                    serviceResponse.Data = "Product added";
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
