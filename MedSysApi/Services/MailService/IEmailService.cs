namespace MedSysApi.Services.MailService
{
    public interface IEmailService
    {
        void SendEmail(CEmailDto request);
    }
}
