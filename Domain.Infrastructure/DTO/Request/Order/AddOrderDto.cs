using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Request.Order
{
    public class AddOrderDto
    {
        public Guid ConsultantsUid { get; set; }

        public List<ProductsDto> Products { get; set; }
    }

    public class ProductsDto
    {
        public Guid Uid { get; set; }
    }

}
