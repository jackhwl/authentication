using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace OwinDemo
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var client = new SendGridClient(ConfigurationManager.AppSettings["sendgrid:Key"]);
            var from = new EmailAddress("jackhwl@hotmail.com");
            var to = new EmailAddress(message.Destination);
            var email = MailHelper.CreateSingleEmail(from, to, message.Subject, message.Body, message.Body);

            await client.SendEmailAsync(email);
        }
    }
}