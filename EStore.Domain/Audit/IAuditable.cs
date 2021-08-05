using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Audit
{
    public interface IAuditable
    {
        
        public Guid Uid { get; set; }

        DateTime CreatedDateTime { get; set; }
        DateTime? LastModifiedDateTime { get; set; }
        bool Deleted { get; set; }
    }
}
