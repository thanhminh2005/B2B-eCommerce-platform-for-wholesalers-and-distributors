using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Banners
{
    public class CreateBannerRequest
    {
        [Required]
        public string DistributorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Image { get; set; }
        [Range(1, 5,
        ErrorMessage = "Banner position must be from range {1} to {2}")]
        public int Position { get; set; }
    }
}
