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
        Task<Response<IQueryable<UserResponse>>> GetUsers(GetUsersRequest request);
        Task<Response<IQueryable<UserResponse>>> GetUsersWithPagination(GetUsersWithPaginationRequest request);
        Task<Response<UserResponse>> GetUserById(GetUserByIdRequest request);
        Task<Response<string>> UpdateUser(UpdateUserRequest request);
            
    }
}
