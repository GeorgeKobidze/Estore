using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Request.Product
{
    public class AddProductDto
    {
        public string Code { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
