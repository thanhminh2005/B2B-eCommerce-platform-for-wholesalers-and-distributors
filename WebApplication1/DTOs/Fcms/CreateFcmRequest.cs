using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Fcms
{
    public class CreateFcmRequest
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
