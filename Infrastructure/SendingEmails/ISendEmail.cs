using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace Infrastructure.SendingEmails
{
    public interface ISendEmail
    {

        Task SendEmailAsync(string mailTo, string subject, string body, string Message ="كود تاكيد الايميل الرجاء عدم مشاركه الكود مع احد");

    }
}



