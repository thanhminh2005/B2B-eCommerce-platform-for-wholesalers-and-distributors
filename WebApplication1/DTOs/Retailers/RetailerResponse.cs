using System;

namespace API.DTOs.Retailers
{
    public class RetailerResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
