using API.DTOs.Notifications;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface INotificationService
    {
        Task<Response<string>> SendNotificationToClients(NotificationToClientsRequest request);
        Task<Response<string>> SendNotificationToRole(NotificationToRoleRequest request);
        Task<Response<IEnumerable<NotificationResponse>>> GetNotifications(GetNotificationsRequest request);
        Task<Response<NotificationResponse>> GetNotificationById(GetNotificationByIdRequest request);
    }
}
