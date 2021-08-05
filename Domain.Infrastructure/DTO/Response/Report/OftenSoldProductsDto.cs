using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Report
{
    public class OftenSoldProductsDto
    {
        public Guid ConsultantUid { get; set; }
        public string ConsultantFirstName { get; set; }
        public string ConsultantLastName { get; set; }
        public string ConsultantIdentification { get; set; }

        public ProductDto OftenSoldProduct { get; set; }
        public ProductDto MostIncomeProduct { get; set; }

    }


    public class ProductDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal CountSum { get; set; }

    }

}
