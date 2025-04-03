using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Infrastructure.Utlities;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
namespace Infrastructure.SendingEmails
{
    public class SendEmails : ISendEmail
    {



        private readonly MailSettings _mailSettings;


        public SendEmails(IOptions<MailSettings> mailSettings)
        {

            _mailSettings = mailSettings.Value;


        }
        public async Task SendEmailAsync(string mailTo, string subject, string body, string message = "كود تاكيد الايميل الرجاء عدم مشاركه الكود مع احد")
        {
            // التحقق من صحة عنوان البريد الإلكتروني باستخدام تعبير عادي (Regular Expression)
            if (string.IsNullOrWhiteSpace(mailTo) || !Regex.IsMatch(mailTo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email address");
            }

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            string emailBody = @"
    <!DOCTYPE html>
    <html lang='ar'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1'>
        <title>تأكيد الحساب</title>
        <style>
            body {
                font-family: Arial, sans-serif;
                background: #f4f4f4;
                padding: 20px;
                direction: rtl;
                text-align: right;
            }
            .container {
                max-width: 600px;
                margin: auto;
                background: #fff;
                padding: 20px;
                border-radius: 10px;
                box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                text-align: center;
            }
            .header {
                background: rgb(54,199,246);
                color: white;
                text-align: center;
                padding: 15px;
                border-radius: 10px 10px 0 0;
                font-size: 22px;
                font-weight: bold;
            }
            .logo {
                width: 100px;
                margin: 10px auto;
            }
            .content {
                padding: 20px;
                color: #333;
                line-height: 1.8;
                font-size: 18px;
            }
            .footer {
                text-align: center;
                font-size: 14px;
                color: #777;
                padding-top: 10px;
                border-top: 1px solid #ddd;
            }
            .btn {
                display: inline-block;
                background: #28a745;
                color: white;
                text-decoration: none;
                padding: 12px 25px;
                border-radius: 5px;
                margin-top: 20px;
                font-size: 18px;
                font-weight: bold;
            }
            .btn:hover {
                background: #218838;
            }
        </style>
    </head>
    <body>
        <div class='container'>
            <img src='https://res.cloudinary.com/ch2p/image/upload/v1741819592/ImagesSearch/zx7wlxefpgbcjxtr0icm.png' alt='شعار الموقع' class='logo'>
            <div class='header'>
                مرحبًا بك في خدمات Chap التجارية
            </div>
            <div class='content'>
                <p>{{Subject}}</p>
                <p>{{Body}}</p>
                <a href='https://www.ch2b.net' class='btn'>
                    يرجى زيارة موقعنا
                </a>
            </div>
            <div class='footer'>
                &copy; {{Year}}
                فريق Chap Company - جميع الحقوق محفوظة
            </div>
        </div>
    </body>
    </html>
    ";

            emailBody = emailBody.Replace("{{Body}}", body)
                                 .Replace("{{Year}}", DateTime.Now.Year.ToString())
                                 .Replace("{{Subject}}", message);

            var builder = new BodyBuilder { HtmlBody = emailBody };
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);
                await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                Console.WriteLine("Email sent successfully.");
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"SMTP Protocol Error: {ex.Message}");
                throw new InvalidOperationException("Failed to send email due to an SMTP protocol error.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw new InvalidOperationException("Failed to send email due to an unexpected error.", ex);
            }
        }


    }
}
