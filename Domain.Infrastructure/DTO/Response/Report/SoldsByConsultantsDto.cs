using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Report
{
    public class SoldsByConsultantsDto
    {
        public Guid ConsultantUid { get; set; }
        public string ConsultantFirstName { get; set; }
        public string ConsultantLastName { get; set; }
        public string ConsultantIdentification { get; set; }
        public decimal OrderSums { get; set; }

        public decimal RefelralSums { get; set; }
    }
}
