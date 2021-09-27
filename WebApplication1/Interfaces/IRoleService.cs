using API.DTOs.Roles;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IRoleService
    {
        Task<Response<string>> CreateRole(CreateRoleRequest request);
        Task<Response<IEnumerable<RoleResponse>>> GetRoles();
        Task<Response<RoleResponse>> GetRoleById(GetRoleByIdRequest request);
        Task<Response<string>> UpdateRole(UpdateRoleRequest request);
    }
}
