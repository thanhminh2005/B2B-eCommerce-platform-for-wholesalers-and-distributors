using API.Warppers;

namespace API.DTOs.Distributors
{
    public class GetDistributorsRequest : PagedRequest
    {
        public bool? IsActive { get; set; }
        public string Search { get; set; }
        public bool? HaveBusinessLicense { get; set; }
    }
}
