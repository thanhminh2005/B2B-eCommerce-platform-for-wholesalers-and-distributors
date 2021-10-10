using API.DTOs.MembershipRanks;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMembershipRankService
    {
        Task<Response<IEnumerable<MembershipRankResponse>>> GetMembershipRanks();
        Task<Response<MembershipRankResponse>> GetMembershipRankById(GetMembershipRankByIdRequest request);
        Task<Response<string>> CreateMembershipRank(CreateMembershipRankRequest request);
        Task<Response<string>> UpdateMembershipRank(UpdateMembershipRankRequest request);
    }
}
