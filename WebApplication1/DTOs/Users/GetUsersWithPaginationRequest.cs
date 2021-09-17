using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Users
{
    public class GetUsersWithPaginationRequest : PagedRequest
    {
        public string SearchValue { get; set; }
        public string RoleId { get; set; }
    }
}
