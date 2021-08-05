using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Report
{
    public class ConsultantsWithOftenSoldProductsDto
    {

        public Guid ConsultantUid { get; set; }
        public string ConsultantFirstName { get; set; }
        public string ConsultantLastName { get; set; }
        public string ConsultantIdentification { get; set; }

        public string ProductCode { get; set; }
        public int ProductCount { get; set; }
    }
}
