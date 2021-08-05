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
    public class Order : AuditTable
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductCount { get; set; }
        public decimal Sum { get; set; }

        public Guid ConsultantsUid { get; set; }
        public  Consultant Consultants { get; set; }

        //public virtual ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
