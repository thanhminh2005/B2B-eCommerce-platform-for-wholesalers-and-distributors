using API.DTOs.Orders;
using API.DTOs.PaymentMethods;
using System;
using System.Collections.Generic;

namespace API.DTOs.Checkouts
{
    public class CheckOutResponse
    {
        public Guid SessionId { get; set; }
        public Guid RetailerId { get; set; }
        public double TotalCost { get; set; }
        public PaymentMethodResponse PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderResponse> OrderResponses { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
