using API.Domains;
using API.DTOs.Users;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserService
    {
        Task<Response<string>> CreateUser(CreateUserRequest request);
        Task<Response<IEnumerable<UserResponse>>> GetUsers(GetUsersRequest request);
        Task<PagedResponse<IEnumerable<UserResponse>>> GetUsersWithPagination(GetUsersWithPaginationRequest request);
        Task<Response<UserResponse>> GetUserById(GetUserByIdRequest request);
        Task<Response<string>> UpdateUserProfile(UpdateUserProfileRequest request);
        Task<Response<string>> UpdateUserPassword(UpdateUserPasswordRequest request);
        Task<Response<UserCountResponse>> GetUserCount();



    }
}
