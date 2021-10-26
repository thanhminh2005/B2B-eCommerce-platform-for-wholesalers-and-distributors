using System.Collections.Generic;

namespace API.DTOs.Notifications
{
    public class NotificationToClientsRequest
    {
        public List<string> UserIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
