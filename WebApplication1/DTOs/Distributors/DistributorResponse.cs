using System;

namespace API.DTOs.Distributors
{
    public class DistributorResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
