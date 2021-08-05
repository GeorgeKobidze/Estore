using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Response.Consultant
{
    public class GetConsultantDto
    {
        public ConsultantDto consultant { get; set; }
        public ConsultantDto Recommendation { get; set; }
        public List<ConsultantDto> RecommendationsGiven { get; set; }
    }

    public class ConsultantDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

}
