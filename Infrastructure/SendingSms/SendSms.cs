using Infrastructure.Utlities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Infrastructure.SendingSms
{
    public class SendSms : ISendSms
    {

        private readonly SMSSetting _SMSSetting;
        public SendSms(IOptions<SMSSetting> options)
        {
            _SMSSetting = options.Value;
        }

        public MessageResource SendOtp(string phoneNumber, string body)
        {
            TwilioClient.Init(_SMSSetting.SID, _SMSSetting.AuthToken);

            var result = MessageResource.Create(

                body: body,
                from: new Twilio.Types.PhoneNumber(_SMSSetting.MyNumber),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
                );


            return result;

        }
    }
}
