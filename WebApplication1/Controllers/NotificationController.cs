using API.Contracts;
using API.DTOs.Notifications;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [RoleAuthorization(Authorization.AD)]
        [HttpPost(ApiRoute.Notifications.Clients)]
        public async Task<IActionResult> Login(NotificationToClientsRequest request)
        {
            var response = await _notificationService.SendNotificationToClients(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [RoleAuthorization(Authorization.AD)]
        [HttpPost(ApiRoute.Notifications.Role)]
        public async Task<IActionResult> Login(NotificationToRoleRequest request)
        {
            var response = await _notificationService.SendNotificationToRole(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Notifications.GetAll)]
        public async Task<IActionResult> GetAll([FromRoute(Name = "user-id")] string userId)
        {
            GetNotificationsRequest request = new GetNotificationsRequest
            {
                UserId = userId
            };
            var response = await _notificationService.GetNotifications(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Notifications.Get)]
        public async Task<IActionResult> Get(GetNotificationByIdRequest request)
        {
            var response = await _notificationService.GetNotificationById(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
