using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Banners
{
    public class DeleteBannerRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
