namespace API.DTOs.Banners
{
    public class CreateBannerRequest
    {
        public string DistributorId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
    }
}
