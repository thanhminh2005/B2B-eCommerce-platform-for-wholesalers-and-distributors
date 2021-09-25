using API.DTOs.Accounts;
using API.Warppers;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IAccountService
    {
        Task<Response<LoginResponse>> Login(LoginRequest request);
    }
}
