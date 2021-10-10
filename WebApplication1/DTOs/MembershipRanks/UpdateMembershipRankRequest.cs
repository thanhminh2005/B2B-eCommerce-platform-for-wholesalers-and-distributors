using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.MembershipRanks
{
    public class UpdateMembershipRankRequest
    {
        public string Id { get; set; }
        public string RankName { get; set; }
    }
}
