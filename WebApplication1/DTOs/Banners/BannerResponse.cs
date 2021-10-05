using System;

namespace API.DTOs.Banners
{
    public class BannerResponse
    {
        public Guid Id { get; set; }
        public Guid DistributorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
    }
}
