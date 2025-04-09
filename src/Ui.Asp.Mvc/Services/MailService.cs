using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;

namespace Ui.Asp.Mvc.Services;

public class MailService(IConfiguration conf, ILogger<MailService> logger)
{
    private readonly IConfiguration _conf = conf;
    private readonly ILogger<MailService> _logger = logger;

    public bool SendEmail(string messageBody, string recipientEmail)
    {
        var providerAddress = _conf["EmailProvider:Address"];
        int providerPort = _conf.GetValue<int>("EmailProvider:Port");
        var apiKey = _conf["EmailProvider:ApiKey"];
        var secret = _conf["EmailProvider:Secret"];
        var senderEmail = _conf["EmailProvider:SenderEmail"];

        if (string.IsNullOrEmpty(providerAddress) || 
            providerPort <= 0 ||
            string.IsNullOrEmpty(apiKey) || 
            string.IsNullOrEmpty(secret) ||
            string.IsNullOrEmpty(senderEmail))
        {
            return false;
        }

        SmtpClient smtpClient = new SmtpClient(providerAddress, providerPort)
        {
            Credentials = new NetworkCredential(apiKey, secret),
            EnableSsl = true
        };

        MailMessage message = new MailMessage(
            senderEmail,
            recipientEmail,
            "Information from Alpha Projects",
            $"This is a informative message from Alpha Project Panel\n{messageBody}"
        );

        try
        {
            smtpClient.Send(message);
            _logger.LogInformation("Email sent successfully.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
