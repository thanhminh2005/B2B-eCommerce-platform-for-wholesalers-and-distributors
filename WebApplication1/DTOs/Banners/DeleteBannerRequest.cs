using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Banners
{
    public class DeleteBannerRequest
    {
        public string Id { get; set; }
        public string DistributorId { get; set; }
    }
}
