using API.Warppers;

namespace API.DTOs.Users
{
    public class GetUsersWithPaginationRequest : PagedRequest
    {
        public string SearchValue { get; set; }
        public string RoleId { get; set; }
    }
}
