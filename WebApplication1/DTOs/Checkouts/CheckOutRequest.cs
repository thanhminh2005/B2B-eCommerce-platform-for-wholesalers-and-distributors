using System.Collections.Generic;

namespace API.DTOs.Checkouts
{
    public class CheckoutRequest
    {
        public List<OrderDetailRequest> Cart { get; set; }
        public string PaymentMethodId { get; set; }
        public string RetailerId { get; set; }
        public string ShippingAddress { get; set; }
    }
}
