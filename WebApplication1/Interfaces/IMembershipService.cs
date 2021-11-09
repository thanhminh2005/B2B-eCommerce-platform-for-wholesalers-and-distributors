using API.DTOs.Memberships;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMembershipService
    {
        Task<PagedResponse<IEnumerable<object>>> GetMemberships(GetMembershipsRequest request);
        Task<Response<MembershipResponse>> GetMembershipById(GetMembershipByIdRequest request);
        Task<Response<string>> CreateMembership(CreateMembershipRequest request);
        Task<Response<string>> UpdateMembership(UpdateMembershipRequest request);
    }
}
