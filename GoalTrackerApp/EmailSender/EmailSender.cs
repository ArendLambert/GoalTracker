using DataAccess.Entities;
using DataAccess.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Timer = System.Timers.Timer;

namespace EmailSender
{
    public class EmailSender
    {
        private readonly Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private IEnumerable<SendEmail> messages = new List<SendEmail>();
        private readonly EmailOptions _options;
        public EmailSender(IServiceProvider serviceProvider, IOptions<EmailOptions> options)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _timer = new Timer(10000);
            _timer.Elapsed += async (sender, e) => await CheckMessagesAsync();
            _timer.Start();
        }

        private async Task CheckMessagesAsync()
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ISendEmailService sendEmailService = scope.ServiceProvider.GetRequiredService<ISendEmailService>();

                messages = await sendEmailService.GetNotSendedAsync();
                if (messages.Count() == 0)
                {
                    return;
                }
                foreach (SendEmail message in messages)
                {
                    if (!message.Sended)
                    {
                        await SendEmail(message);
                    }
                }
            }
        }

        private async Task SendEmail(SendEmail inputMessage)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                try
                {
                    ISendEmailService sendEmailService = scope.ServiceProvider.GetRequiredService<ISendEmailService>();
                    MimeMessage message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(_options.Email));
                    if (inputMessage.GoalEmails.Count() == 0)
                    {
                        return;
                    }
                    message.To.Add(MailboxAddress.Parse(inputMessage.GoalEmails.First().IdGoalNavigation.IdUserNavigation.Email));
                    message.Subject = inputMessage.GoalEmails.First().IdGoalNavigation.Title;
                    message.Body = new TextPart("html") { Text = $"<h2>{inputMessage.Message}</h2>" };

                    using var client = new SmtpClient();
                    await client.ConnectAsync(_options.SMTP, _options.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_options.Email, _options.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    await sendEmailService.UpdateAsync(new Core.Models.SendEmailModel(inputMessage.Id, inputMessage.Date,
                        inputMessage.Message, true));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
