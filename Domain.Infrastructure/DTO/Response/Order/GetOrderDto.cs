using Domain.Infrastructure.DTO.Response.Consultant;
using Domain.Infrastructure.DTO.Response.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Order
{
    public class GetOrderDto
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductCount { get; set; }
        public decimal Sum { get; set; }

        public ConsultantDto Consultant { get; set; }
        public List<GetProductDto> Products { get; set; }

    }
}
