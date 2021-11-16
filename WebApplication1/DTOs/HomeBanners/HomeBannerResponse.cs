using System;

namespace API.DTOs.HomeBanners
{
    public class HomeBannerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
    }
}
