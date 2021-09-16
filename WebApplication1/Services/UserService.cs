using API.DTOs.Users;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Task<Response<string>> CreateUser(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<UserResponse>> GetUserById(GetUserByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<UserResponse>>> GetUsers(GetUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<UserResponse>>> GetUsersWithPagination(GetUsersWithPaginationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> UpdateUser(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
