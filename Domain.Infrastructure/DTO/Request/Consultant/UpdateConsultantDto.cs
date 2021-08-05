using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Request.Consultant
{
    public class UpdateConsultantDto
    {
        public Guid Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Gender { get; set; }

        [DefaultValue("2012/12/03")]
        public string DateOfBirth { get; set; }
        public Guid? ConsultantUid { get; set; }
    }
}
