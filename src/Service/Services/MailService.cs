

using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Services;

public class MailService(IConfiguration conf, ILogger<MailService> logger) : IMailService
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
            .WithHtmlPart($"<h6>Alpha Project Panel</h6><br>{messageBody}")
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
            _logger.LogWarning($"MailService failed to send Email: {ex}");
            return false;
        }
    }
}
