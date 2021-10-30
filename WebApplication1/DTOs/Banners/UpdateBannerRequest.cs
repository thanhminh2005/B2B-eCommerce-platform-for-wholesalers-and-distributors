using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Banners
{
    public class UpdateBannerRequest
    {
        [Required]
        public string Id { get; set; }
        public string DistributorId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
    }
}
