using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Infrastructure.Utlities;
namespace Infrastructure.SendingEmails
{
    public class SendEmails : ISendEmail
    {



        private readonly MailSettings _mailSettings;


        public SendEmails(IOptions<MailSettings> mailSettings)
        {

            _mailSettings = mailSettings.Value;


        }
        public async Task SendEmailAsync(string mailTo, string subject, string body,string Message= "كود تاكيد الايميل الرجاء عدم مشاركه الكود مع احد")
        {

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            string baseDirectory = AppContext.BaseDirectory;

            string projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));

            string templatePath = Path.Combine(projectRoot, "Infrastructure", "Utlities", "Templates", "emailTemplate.html");

            string emailBody = File.ReadAllText(templatePath);

            emailBody = emailBody.Replace("{{Body}}", body)
                                 .Replace("{{Year}}", DateTime.Now.Year.ToString()).Replace("{{Subject}}", Message);
           

            var builder = new BodyBuilder
            {

                HtmlBody = emailBody

            };

            email.Body = builder.ToMessageBody();


            try
            {

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
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
