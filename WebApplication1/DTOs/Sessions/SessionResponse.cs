using API.DTOs.PaymentMethods;
using System;

namespace API.DTOs.Sessions
{
    public class SessionResponse
    {
        public Guid Id { get; set; }
        public Guid RetailerId { get; set; }
        public string RetailerName { get; set; }
        public PaymentMethodResponse PaymentMethod { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
