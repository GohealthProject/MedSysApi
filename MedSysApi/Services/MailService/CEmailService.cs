
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Diagnostics;

namespace MedSysApi.Services.MailService
{
    public class CEmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public CEmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(CEmailDto request)
        {
            string body = "<img src=\"https://i.imgur.com/l1mgvEG.png\"><br/>親愛的新會員您好<br/><br/>很榮幸你加入GO健康的會員，請點擊以下連結驗證您的GO健康帳號。<br/><br/>  https://localhost:7203/Accout/";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.Address));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            //email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, false);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
