using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ui.Asp.Mvc.Services;

public class MailService(IConfiguration conf, ILogger<MailService> logger)
{
    private readonly IConfiguration _conf = conf;
    private readonly ILogger<MailService> _logger = logger;

    public async Task<bool> SendEmail(string messageBody, string recipientEmail)
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

        MailjetClient client = new MailjetClient(apiKey, secret);
        MailjetRequest request = new MailjetRequest { Resource = Send.Resource };

        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact(senderEmail))
            .WithTo(new SendContact(recipientEmail))
            .WithSubject("Information from Alpha Projects")
            .WithHtmlPart($"<h1>Alpha Project Panel</h1><h2>If you did not request this information, please do NOT click the link</h2><br><p>{messageBody}</p>")
            .Build();

        try
        {
            var response = await client.SendTransactionalEmailAsync(email);
            if (response.Messages.Length > 0)
                return true;

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"MailService failed to send Email: {ex}");
            return false;
        }


        #region SmtpClient is not recommended for new projects, see documentation
        //  https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=net-9.0
        //  https://github.com/dotnet/platform-compat/blob/master/docs/DE0005.md

        //SmtpClient smtpClient = new SmtpClient(providerAddress, providerPort)
        //{
        //    Credentials = new NetworkCredential(apiKey, secret),
        //    EnableSsl = true
        //};

        //MailMessage message = new MailMessage(
        //    senderEmail,
        //    recipientEmail,
        //    "Information from Alpha Projects",
        //    $"This is a informative message from Alpha Project Panel\n{messageBody}"
        //);

        //try
        //{
        //    smtpClient.Send(message);
        //    _logger.LogInformation("Email sent successfully.");
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogInformation($"Error sending email: {ex.Message}");
        //    return false;
        //}
        #endregion
    }
}
