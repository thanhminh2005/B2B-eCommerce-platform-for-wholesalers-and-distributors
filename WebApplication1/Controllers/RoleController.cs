using API.Contracts;
using API.DTOs.Roles;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [RoleAuthorization(Authorization.AD)]
        [HttpPost(ApiRoute.Roles.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            var response = await _roleService.CreateRole(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Roles.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetRoleByIdRequest request = new GetRoleByIdRequest
            {
                Id = id
            };
            var response = await _roleService.GetRoleById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Roles.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _roleService.GetRoles();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Roles.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateRoleRequest request)
        {
            var response = await _roleService.UpdateRole(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
