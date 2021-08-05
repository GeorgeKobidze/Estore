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
    public class OrderDetails : AuditTable
    {

        public Guid ProductsUid { get; set; }
        public Product Products { get; set; }

        public Guid OrdersUid { get; set; }
        public Order Orders { get; set; }

    }
}
