using API.Warppers;

namespace API.DTOs.Memberships
{
    public class GetMembershipsRequest : PagedRequest
    {
        public string RetailerId { get; set; }
        public string DistributorId { get; set; }
        public string MembershipRankId { get; set; }
    }
}
