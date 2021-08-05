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
    public class Product : AuditTable
    {
        public string Code { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        //public virtual  ICollection<OrderDetails> OrderDetails { get; set; }

    }
}
