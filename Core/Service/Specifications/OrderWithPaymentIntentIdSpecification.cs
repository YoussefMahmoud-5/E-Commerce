using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.OrderModule;

namespace Service.Specifications
{
    internal class OrderWithPaymentIntentIdSpecification : BaseSpecification<Order,Guid>
    {
        public OrderWithPaymentIntentIdSpecification(string paymentIntentId)
                                 : base(O => O.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
