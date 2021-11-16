using System.ComponentModel.DataAnnotations;

namespace API.DTOs.HomeBanners
{
    public class DeleteHomeBannerRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
