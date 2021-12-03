using API.Contracts;
using API.Domains;
using API.DTOs.Distributors;
using API.DTOs.Retailers;
using API.DTOs.Users;
using API.Helpers;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Response<string>> CreateUser(CreateUserRequest request)
        {
            if (request != null)
            {
                var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(request.Username));
                if (user == null)
                {
                    var emailExist = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Email.Equals(request.Email));
                    if (emailExist == null)
                    {
                        var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(Guid.Parse(request.RoleId));
                        if (role != null)
                        {
                            User newUser = _mapper.Map<User>(request);
                            if (string.IsNullOrWhiteSpace(request.DoB))
                            {
                                newUser.DoB = DateTime.MinValue;
                            }
                            else
                            {
                                newUser.DoB = DateConverter.StringToDateTime(request.DoB);
                            }
                            newUser.RoleId = role.Id;
                            newUser.DateCreated = DateTime.UtcNow;
                            byte[] passwordHash, passwordSalt;
                            PasswordHash.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
                            newUser.PasswordHash = passwordHash;
                            newUser.PasswordSalt = passwordSalt;
                            newUser.Id = Guid.NewGuid();
                            newUser.IsActive = false;
                            newUser.ActivationCode = Guid.NewGuid();
                            await _unitOfWork.GetRepository<User>().AddAsync(newUser);
                            await _unitOfWork.SaveAsync();
                            if (role.Name.Equals(Authorization.RT))
                            {
                                var retailer = new Retailer
                                {
                                    DateCreated = DateTime.UtcNow,
                                    IsActive = false,
                                    Id = Guid.NewGuid(),
                                    UserId = newUser.Id,
                                };
                                await _unitOfWork.GetRepository<Retailer>().AddAsync(retailer);
                            }
                            if (role.Name.Equals(Authorization.DT))
                            {
                                var distributor = new Distributor
                                {
                                    DateCreated = DateTime.UtcNow,
                                    IsActive = false,
                                    Id = Guid.NewGuid(),
                                    UserId = newUser.Id,
                                };
                                await _unitOfWork.GetRepository<Distributor>().AddAsync(distributor);
                            }
                            await _unitOfWork.SaveAsync();
                            var emailcheck = false;
                            if (!role.Name.Equals(Authorization.AD))
                            {
                                emailcheck = await EmailSender.SendAsync(_configuration, role.Name, newUser.Email, newUser.ActivationCode.ToString());
                            }
                            return new Response<string>(newUser.Id.ToString(), message: "Account Created");
                        }
                    }
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
        public async Task<Response<bool>> CheckEmailAvailable(string email)
        {
            var check = false;
            var emailExist = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Email.Equals(email));
            if (emailExist == null)
            {
                return new Response<bool>(check, message: "Email Available");
            }
            return new Response<bool>(check, message: "Email not Available");
        }

        public async Task<Response<bool>> CheckUsernameAvailable(string username)
        {
            var check = false;
            var usernameExist = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(username));
            if (usernameExist == null)
            {
                return new Response<bool>(check, message: "Username Available");
            }
            return new Response<bool>(check, message: "Username not Available");
        }

        public async Task<Response<UserCountResponse>> GetUserCount()
        {
            var user = await _unitOfWork.GetRepository<User>().CountAsync();
            return new Response<UserCountResponse>(data: new UserCountResponse { TotalUser = user }, message: "Success");
        }

        public async Task<Response<IEnumerable<UserResponse>>> GetUsers(GetUsersRequest request)
        {
            var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
            if (users.Any())
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
                                                                                     orderBy: x => x.OrderBy(y => y.Username),
                                                                                     includeProperties: "Retailers,Distributors");


            var totalcount = await _unitOfWork.GetRepository<User>().CountAsync(filter: x => (request.RoleId == null || x.RoleId.Equals(Guid.Parse(request.RoleId)))
                                                                                     && (request.SearchValue == null || x.Username.Contains(request.SearchValue)));
            var response = _mapper.Map<IEnumerable<UserResponse>>(users);
            foreach (var user in response)
            {
                if (users.FirstOrDefault(x => x.Id.Equals(user.Id)).Distributors.Count > 0)
                {
                    user.Distributor = _mapper.Map<UserDistributorResponse>(users.FirstOrDefault(x => x.Id.Equals(user.Id)).Distributors.ElementAt(0));
                }
                if (users.FirstOrDefault(x => x.Id.Equals(user.Id)).Retailers.Count > 0)
                {
                    user.Retailer = _mapper.Map<UserRetailerResponse>(users.FirstOrDefault(x => x.Id.Equals(user.Id)).Retailers.ElementAt(0));
                }
            }
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
                        if (request.NewPassword.Length >= 6)
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
                user.TaxId = request.TaxId;
                user.Address = request.Address;
                user.Avatar = request.Avatar;
                user.DisplayName = request.DisplayName;
                user.DoB = DateConverter.StringToDateTime(request.DoB);
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.Sex = request.Sex;
                user.IsActive = request.IsActive;
                user.BusinessLicense = request.BusinessLicense;
                user.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<User>().UpdateAsync(user);
                await _unitOfWork.SaveAsync();
                return new Response<string>(user.Username, message: "Update Profile Successfully");
            }
            return new Response<string>(message: "Failed To Update Profile");
        }

        public async Task<Response<string>> VertifiedUser(string ActivateCode)
        {
            var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.ActivationCode.Equals(Guid.Parse(ActivateCode)));
            if (user != null)
            {
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    user.DateModified = DateTime.UtcNow;
                    user.ActivationCode = null;
                    _unitOfWork.GetRepository<User>().UpdateAsync(user);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(user.Username, message: "Account Vertified");
                }
            }
            return new Response<string>(message: "Failed To Vertified");
        }

        public async Task<Response<string>> CreateNewUserPassword(CreateNewUserPasswordRequest request)
        {
            var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(request.Username) && x.Email.Equals(request.Email));
            if (user != null)
            {
                byte[] passwordHash, passwordSalt;
                var password = PasswordHash.GenerateRandomAlphanumericString(12);
                PasswordHash.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<User>().UpdateAsync(user);
                await _unitOfWork.SaveAsync();
                bool checkEmail = await EmailSender.SendPasswordAsync(_configuration, user.Email, password);
                return new Response<string>(user.Username, message: "Create New Password Successfully, Check Email For New Password");
            }
            return new Response<string>(message: "Failed To Update Password");
        }
    }
}
