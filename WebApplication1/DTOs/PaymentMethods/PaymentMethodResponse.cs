using System;

namespace API.DTOs.PaymentMethods
{
    public class PaymentMethodResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
