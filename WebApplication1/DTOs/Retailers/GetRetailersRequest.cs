using API.Warppers;

namespace API.DTOs.Retailers
{
    public class GetRetailersRequest : PagedRequest
    {
        public bool? IsActive { get; set; }
    }
}
