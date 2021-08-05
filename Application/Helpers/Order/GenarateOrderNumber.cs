using Domain.Infrastructure.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Application.Helpers.Orders
{
    public class GenarateOrderNumber
    {
        private readonly IUnitOfWork<Order> _iunitofWork;

        public GenarateOrderNumber(IUnitOfWork<Order> iunitofWork)
        {
            _iunitofWork = iunitofWork;
        }

        public string GenerateUniqueOrdernumber()
        {   

            string order = _iunitofWork.Repository.GetAll().Result.Select(e => e.OrderNumber).LastOrDefault();
            string newOrder;

            if (order != null)
            {
                var num = Convert.ToInt32(order.Substring(6)) + 1;
                newOrder = "Order-" + num.ToString();
            }
            else
            {
                newOrder = "Order-1";
            }

            return newOrder;

        } 

    }
}
