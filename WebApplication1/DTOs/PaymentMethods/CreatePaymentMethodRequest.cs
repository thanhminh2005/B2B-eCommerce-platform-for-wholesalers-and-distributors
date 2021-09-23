using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.PaymentMethods
{
    public class CreatePaymentMethodRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
