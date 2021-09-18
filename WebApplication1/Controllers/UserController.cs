using API.Contracts;
using API.DTOs.Users;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost(ApiRoute.Users.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var response = await _userService.CreateUser(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Users.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetUserByIdRequest request = new GetUserByIdRequest();
            request.Id = id;
            var response = await _userService.GetUserById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Users.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetUsersRequest request)
        {
            var response = await _userService.GetUsers(request);
            if(response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Users.Count)]
        public async Task<IActionResult> GetCount()
        {
            var response = await _userService.GetUserCount();
            return Ok(response);
        }

        [HttpGet(ApiRoute.Users.GetPaging)]
        public async Task<IActionResult> GetPaging([FromQuery] GetUsersWithPaginationRequest request)
        {
            var response = await _userService.GetUsersWithPagination(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Users.UpdatePassword)]
        public async Task<IActionResult> Update([FromBody] UpdateUserPasswordRequest request)
        {
            var response = await _userService.UpdateUserPassword(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.Users.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateUserProfileRequest request)
        {
            var response = await _userService.UpdateUserProfile(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
