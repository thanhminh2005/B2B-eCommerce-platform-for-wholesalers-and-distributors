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

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateUser(CreateUserRequest request)
        {
            if (request != null)
            {
                var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(request.Username));
                if (user == null)
                {
                    User newUser = _mapper.Map<User>(request);
                    newUser.DoB = DateConverter.StringToDateTime(request.DoB);
                    newUser.RoleId = Guid.Parse(request.RoleId);
                    newUser.DateCreated = DateTime.UtcNow;
                    byte[] passwordHash, passwordSalt;
                    PasswordHash.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
                    newUser.PasswordHash = passwordHash;
                    newUser.PasswordSalt = passwordSalt;
                    newUser.Id = Guid.NewGuid();
                    newUser.IsActive = true;
                    await _unitOfWork.GetRepository<User>().AddAsync(newUser);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(newUser.Username, message: "User Registered.");
                }
            }
            return new Response<string>(message: "Failed to Register");
        }

        public async Task<Response<UserResponse>> GetUserById(GetUserByIdRequest request)
        {
            if (request != null)
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(Guid.Parse(request.Id));
                if (user != null)
                {
                    return new Response<UserResponse>(_mapper.Map<UserResponse>(user), message: "Succeed");
                }
            }
            return new Response<UserResponse>(message: "User Not Found");
        }

        public async Task<Response<UserCountResponse>> GetUserCount()
        {
            var user = await _unitOfWork.GetRepository<User>().CountAsync();
            return new Response<UserCountResponse>(data: new UserCountResponse { TotalUser = user }, message: "Success");
        }

        public async Task<Response<IEnumerable<UserResponse>>> GetUsers(GetUsersRequest request)
        {
            var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
            if (users.Count() != 0)
            {
                if (!string.IsNullOrWhiteSpace(request.RoleId))
                {
                    var roleId = Guid.Parse(request.RoleId);
                    users = users.Where(x => x.RoleId.Equals(roleId));
                }
                return new Response<IEnumerable<UserResponse>>(_mapper.Map<IEnumerable<UserResponse>>(users), message: "Success");
            }
            return new Response<IEnumerable<UserResponse>>(message: "Empty");

        }

        public async Task<PagedResponse<IEnumerable<UserResponse>>> GetUsersWithPagination(GetUsersWithPaginationRequest request)
        {
            var users = await _unitOfWork.GetRepository<User>().GetPagedReponseAsync(request.PageNumber,
                                                                                     request.PageSize,
                                                                                     filter: x =>
                                                                                     (request.RoleId == null || x.RoleId.Equals(Guid.Parse(request.RoleId)))
                                                                                     && (request.SearchValue == null || x.Username.Contains(request.SearchValue)),
                                                                                     orderBy: x => x.OrderBy(y => y.Username));

            var totalcount = await _unitOfWork.GetRepository<User>().CountAsync(filter: x => (request.RoleId == null || x.RoleId.Equals(Guid.Parse(request.RoleId)))
                                                                                     && (request.SearchValue == null || x.Username.Contains(request.SearchValue)));
            var response = _mapper.Map<IEnumerable<UserResponse>>(users);
            return new PagedResponse<IEnumerable<UserResponse>>(response, request.PageNumber, request.PageSize, totalcount);
        }

        public async Task<Response<string>> UpdateUserPassword(UpdateUserPasswordRequest request)
        {
            var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.UserId)));
            if (user != null)
            {

                if (!string.IsNullOrWhiteSpace(request.OldPassword) && PasswordHash.VerifyPasswordHash(request.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    if (!string.IsNullOrWhiteSpace(request.NewPassword) && !string.IsNullOrWhiteSpace(request.ComfirmPassword))
                    {
                        if (request.NewPassword.Length > 8)
                        {
                            byte[] passwordHash, passwordSalt;
                            PasswordHash.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);
                            user.PasswordHash = passwordHash;
                            user.PasswordSalt = passwordSalt;
                            user.DateModified = DateTime.UtcNow;
                            _unitOfWork.GetRepository<User>().UpdateAsync(user);
                            await _unitOfWork.SaveAsync();
                            return new Response<string>(user.Username, message: "Update Password Successfully");
                        }
                    }
                }
            }
            return new Response<string>(message: "Failed To Update Password");
        }

        public async Task<Response<string>> UpdateUserProfile(UpdateUserProfileRequest request)
        {
            var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
            if (user != null)
            {
                user.Address = request.Address;
                user.Avatar = request.Avatar;
                user.DisplayName = request.DisplayName;
                user.DoB = DateConverter.StringToDateTime(request.DoB);
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Sex = request.Sex;
                user.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<User>().UpdateAsync(user);
                await _unitOfWork.SaveAsync();
                return new Response<string>(user.Username, message: "Update Profile Successfully");
            }
            return new Response<string>(message: "Failed To Update Profile");
        }
    }
}
