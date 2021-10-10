using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.MembershipRanks
{
    public class MembershipRankResponse
    {
        public Guid Id { get; set; }
        public string RankName { get; set; }
    }
}
