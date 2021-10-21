using API.Domains;
using API.DTOs.Accounts;
using API.DTOs.Roles;
using API.Helpers;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using B2B.AppSettings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<Response<LoginResponse>> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new Response<LoginResponse>("Username and Password must be required");
            }

            var user = await _unitOfWork.GetRepository<User>().FirstAsync(x => x.Username.Equals(request.Username));
            var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(user.RoleId);
            if (user != null)
            {

                if (PasswordHash.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                {
                    if (!user.IsActive)
                    {
                        return new Response<LoginResponse>(message: "This Account has be deactivated");
                    }
                    JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
                    LoginResponse response = new LoginResponse
                    {
                        Avatar = user.Avatar,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Id = user.Id,
                        PhoneNumber = user.PhoneNumber,
                        Role = _mapper.Map<RoleResponse>(role),
                        Username = user.Username,
                        JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
                    };
                    //var fcm = new Fcm
                    //{
                    //    DateCreated = DateTime.Now,
                    //    Id = Guid.NewGuid(),
                    //    Token = "",
                    //    UserId = response.Id
                    //};
                    //await _unitOfWork.GetRepository<Fcm>().AddAsync(fcm);
                    //await _unitOfWork.SaveAsync();
                    return new Response<LoginResponse>(response, message: "Login Successed");
                }

            }
            return new Response<LoginResponse>(message: "Invalid Username or Password");
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(User user)
        {
            var role = await _unitOfWork.GetRepository<Role>().FirstAsync(x => x.Id.Equals(user.RoleId));
            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Role, role.Name),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
