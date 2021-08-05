using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Request.Order
{
    public class UpdateOrderDto
    {
        public string OrderNumber { get; set; }
        public Guid Consultant { get; set; }
        public List<ProductsDto> NewProducts { get; set; }
        public List<ProductsDto> ProductsForDeletion { get; set; }
    }

}
