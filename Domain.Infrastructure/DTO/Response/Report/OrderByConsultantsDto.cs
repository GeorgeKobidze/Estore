using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Report
{
    public class OrderByConsultantsDto
    {
        public Guid OrderUid { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public Guid ConsultantUid { get; set; }
        public string ConsultantFirstName { get; set; }
        public string ConsultantLastName { get; set; }
        public string ConsultantIdentification { get; set; }

        public int ProductCount { get; set; }
        public decimal OrderSum { get; set; }
    }
}
