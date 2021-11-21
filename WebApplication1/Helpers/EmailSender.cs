using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class EmailSender
    {
        public static async Task<bool> SendAsync(IConfiguration configuration, string to, string activationCode)
        {
            var apiKey = configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("haru_konshita@hotmail.com", "The Sender");
            var toEmail = new EmailAddress(to, "The Receiver");

            var subject = "Test Email with SendGrid";
            var htmlContent = "<a href=\"https://gecko-b3c27.web.app/ss/email-vertified?activationCode=" + activationCode + "\"><button class=\"button\">Click Here to Vertified</button></a>";
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, htmlContent);
            try
            {
                await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return false;
            }
            return true;
        }
    }
}
