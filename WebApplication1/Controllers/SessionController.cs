using API.Contracts;
using API.DTOs.Sessions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService userService)
        {
            _sessionService = userService;
        }

        [HttpPost(ApiRoute.Sessions.Create)]
        public async Task<IActionResult> Create([FromBody] CreateSessionRequest request)
        {
            var response = await _sessionService.CreateSession(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Sessions.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetSessionByIdRequest request = new GetSessionByIdRequest
            {
                Id = id
            };
            var response = await _sessionService.GetSessionById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Sessions.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetSessionsRequest request)
        {
            var response = await _sessionService.GetSessions(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpPut(ApiRoute.Sessions.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateSessionRequest request)
        {
            var response = await _sessionService.UpdateSession(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.Sessions.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            DeleteSessionRequest request = new DeleteSessionRequest
            {
                Id = id
            };
            var response = await _sessionService.DeleteSession(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }
    }
}
