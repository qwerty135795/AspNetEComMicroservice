using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using OrderingApplication.Contracts.Infrastructure;
using OrderingApplication.Models;

namespace OrderingInfrastructure.Mail;
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }
    public async Task<bool> SendEmail(Email email)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
        message.To.Add(new MailboxAddress(email.To,email.To));
        message.Subject = email.Subject;
        message.Body = new BodyBuilder() { TextBody = email.Body}.ToMessageBody();
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com",587,false);
        client.Authenticate("sapphiremessanger@gmail.com",_settings.Password);
        await client.SendAsync(message);
        client.Disconnect(true);
        return true;
    }
}
