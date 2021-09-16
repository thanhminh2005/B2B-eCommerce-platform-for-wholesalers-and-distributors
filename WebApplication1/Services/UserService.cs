using API.Domains;
using API.DTOs.Users;
using API.Helpers;
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
        public async Task<Response<string>> CreateUser(CreateUserRequest request)
        {
            if(request != null)
            {

                var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(request.Username));
                if(user != null)
                {
                    User newUser = _mapper.Map<User>(request);
                    newUser.DateCreated = DateTime.UtcNow;
                    byte[] passwordHash, passwordSalt;
                    PasswordHash.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
                    newUser.PasswordHash = passwordHash;
                    newUser.PasswordSalt = passwordSalt;
                    newUser.Id = Guid.NewGuid();
                    await _unitOfWork.GetRepository<User>().AddAsync(newUser);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(user.Username, message: "User Registered.");
                }
            }
            return new Response<string>(message: "Failed to Register");
        }

        public Task<Response<UserResponse>> GetUserById(GetUserByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IQueryable<UserResponse>>> GetUsers(GetUsersRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IQueryable<UserResponse>>> GetUsersWithPagination(GetUsersWithPaginationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> UpdateUser(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
