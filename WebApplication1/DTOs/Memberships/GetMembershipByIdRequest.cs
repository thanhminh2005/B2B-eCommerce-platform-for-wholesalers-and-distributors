using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.DTOs.Memberships
{
    public class GetMembershipByIdRequest
    {
        public string RetailerId { get; set; }
        public string DistributorId { get; set; }
    }
}
