﻿namespace API.DTOs.Orders
{
    public class GetOrdersRequest
    {
        public string SessionId { get; set; }
        public string DistributorId { get; set; }
        public int? Status { get; set; }
    }
}
