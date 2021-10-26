using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class Notification
    {
        public static async Task<string> SendNotifications(List<string> clientTokens, string title, string description)
        {
            var message = new MulticastMessage()
            {
                Tokens = clientTokens,
                Data = new Dictionary<string, string>()
                {
                    {"Title", title},
                    {"Decription", description},
                },
            };
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message).ConfigureAwait(true);
            if (response.FailureCount > 0)
            {
                return "Some notification not get to the receiver";
            }
            return "Succeed all";
        }
    }
}
