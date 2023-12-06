using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(string body,string address)
        {            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("gohealth790@gmail.com"));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "【GO健康】帳號驗證通知信";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, false);
            smtp.Authenticate("gohealth790@gmail.com", "aylk gntm dwab njzp");
            smtp.Send(email);
            smtp.Disconnect(true);

            if (email == null)
            {
                return BadRequest();
            }



            return Ok();
        }
    }
}
