using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.DTO.Common
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Succes { get; set; } = true;
        public List<string> Message { get; set; } = new List<string>();
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
