using API.DTOs.Memberships;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMembershipService
    {
        Task<PagedResponse<IEnumerable<MembershipResponse>>> GetMemberships(GetMembershipsRequest request);
        Task<Response<MembershipResponse>> GetMembershipById(GetMembershipByIdRequest request);
        Task<Response<string>> CreateMembership(CreateMembershipRequest request);
        Task<Response<string>> UpdateMembership(UpdateMembershipRequest request);
    }
}
