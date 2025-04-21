namespace Service.Interfaces;
public interface IMailService
{
    Task<bool> SendEmail(string messageBody, string recipientEmail);
}