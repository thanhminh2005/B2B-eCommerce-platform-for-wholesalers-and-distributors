using API.DTOs.Accounts;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAccountService
    {
        Task<Response<LoginResponse>> Login(LoginRequest request); 
    }
}
