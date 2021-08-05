using Domain.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Consultant : AuditTable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Guid? ConsultantUid { get; set; }
        public  Consultant  Recomendator { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
