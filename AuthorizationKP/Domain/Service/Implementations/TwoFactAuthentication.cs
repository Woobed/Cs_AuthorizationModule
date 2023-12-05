using AuthorizationKP.Domain.Service.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using System.Net.Mail;


namespace AuthorizationKP.Domain.Service.Implementations
{
    public class TwoFactAuthentication : ITwoFactAuthentication
    {
        private readonly ILogger<TwoFactAuthentication> logger;
        public TwoFactAuthentication(ILogger<TwoFactAuthentication> logger)
        {
            this.logger = logger;
        }
        public string SendConfirmCode(string adress)
        {
            try
            {                                                                           
                Random rnd = new Random();
                string confirmCode = rnd.Next(100000,999999).ToString();
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sapris@vgtu.ru", "aufkp2@yandex.ru"));
                message.To.Add(new MailboxAddress("", adress));
                message.Subject = "Подтверждение адреса электронной почты";
                message.Body = new BodyBuilder() { HtmlBody = $"<div>Код для подтверждения: {confirmCode}</div>" }.ToMessageBody();
                using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtpClient.Connect("smtp.yandex.ru", 25, false);
                    smtpClient.Authenticate("aufkp2@yandex.ru", "iptobppypkocqtvu");

                    smtpClient.Send(message);

                    smtpClient.Disconnect(true);
                    logger.LogInformation("Сообщение отправлено успешно");
                    return confirmCode;
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.GetBaseException().Message);
                return "0";
            }
        }

    }
}
