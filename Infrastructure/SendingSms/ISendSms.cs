using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.SendingSms
{
    public interface ISendSms
    {


        MessageResource SendOtp(string phoneNumber, string body);

    }
}
