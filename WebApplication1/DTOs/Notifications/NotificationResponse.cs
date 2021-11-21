using System;

namespace API.DTOs.Notifications
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
