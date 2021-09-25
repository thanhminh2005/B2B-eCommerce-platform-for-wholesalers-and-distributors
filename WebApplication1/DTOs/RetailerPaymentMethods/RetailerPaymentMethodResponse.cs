using API.DTOs.PaymentMethods;
using System;

namespace API.DTOs.RetailerPaymentMethods
{
    public class RetailerPaymentMethodResponse
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public PaymentMethodResponse PaymentMethod { get; set; }
        public string Detail { get; set; }
    }
}
