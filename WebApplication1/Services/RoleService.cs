using API.Domains;
using API.DTOs.Roles;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateRole(CreateRoleRequest request)
        {
            if(!string.IsNullOrWhiteSpace(request.Name))
            {
                var role = await _unitOfWork.GetRepository<Role>().FirstAsync(c => c.Name.Equals(request.Name));
                if(role == null)
                {
                    var newRole = _mapper.Map<Role>(request);
                    newRole.Id = Guid.NewGuid();
                    newRole.DateCreated = DateTime.UtcNow;
                    await _unitOfWork.GetRepository<Role>().AddAsync(newRole);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "Role Created");
                }
            }
            return new Response<string>(message: "Failed to Create");
        }

        public async Task<Response<RoleResponse>> GetRoleById(GetRoleByIdRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Id))
            {
                var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(Guid.Parse(request.Id));
                if(role != null)
                {
                    return new Response<RoleResponse>(_mapper.Map<RoleResponse>(role), message: "Success");
}
}
            return new Response<RoleResponse>(message: "Role not Found");
        }

        public async Task<Response<IEnumerable<RoleResponse>>> GetRoles()
        {
            var roles = await _unitOfWork.GetRepository<Role>().GetAllAsync();
            if (roles != null)
            {
                return new Response<IEnumerable<RoleResponse>>(_mapper.Map<IEnumerable<RoleResponse>>(roles), message: "Success");
            }
            return new Response<IEnumerable<RoleResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateRole(UpdateRoleRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id) && !string.IsNullOrWhiteSpace(request.Name))
            {
                var role = await _unitOfWork.GetRepository<Role>().GetByIdAsync(Guid.Parse(request.Id));
                if (role != null)
                {
                    role.DateModified = DateTime.UtcNow;
                    role.Name = request.Name;
                    _unitOfWork.GetRepository<Role>().UpdateAsync(role);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(request.Name, message: "Role Updated");
                }
            }
            return new Response<string>(message: "Role not Found");
        }
    }
}
