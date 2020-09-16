namespace ActiveIS.UmbracoForms.CustomEmailV7.Interfaces
{
    public interface ISmtpService
    {
        void SendEmail(string emailBody, string toEmail, string fromEmail, string fromName, string subject,
            string replyTo, string bcc, string cc);
    }
}
