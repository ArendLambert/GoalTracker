using Core.Interfaces;
using Core.Models;
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
        private IEnumerable<Goal> goalsDeadline = new List<Goal>();
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
                IGoalService goalService = scope.ServiceProvider.GetRequiredService<IGoalService>();
                IGoalEmailService goalEmailService = scope.ServiceProvider.GetRequiredService<IGoalEmailService>();
                IDateTimeManager dateTimeManager = scope.ServiceProvider.GetRequiredService<IDateTimeManager>();

                goalsDeadline = await goalService.GetDeadlineAsync();
                if (goalsDeadline.Count() != 0)
                {
                    foreach (Goal goal in goalsDeadline)
                    {
                        await SendEmailAboutDeadline(goal, goalEmailService, sendEmailService, dateTimeManager);
                    }
                }
                
                messages = await sendEmailService.GetNotSendedAsync();
                if (messages.Count() != 0)
                {
                    foreach (SendEmail message in messages)
                    {
                        if (!message.Sended)
                        {
                            await SendEmail(message, sendEmailService);
                        }
                    }
                }              
            }
        }

        private async Task SendEmail(SendEmail inputMessage, ISendEmailService sendEmailService)
        {
            try
            {
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

        private async Task SendEmailAboutDeadline(Goal goal, IGoalEmailService goalEmailService, ISendEmailService sendEmailService, IDateTimeManager dateTimeManager)
        {
            try
            {
                ICollection<GoalEmailModel> goalEmails = await goalEmailService.GetByIdGoalAsync(goal.Id);
                if (goalEmails.Count == 0)
                {
                    MimeMessage message = new MimeMessage();
                    string messageText;
                    if (string.IsNullOrEmpty(goal.Punishment))
                    {
                        messageText = $"<h2>Дедлайн прошел: {goal.Deadline}!<br>";
                    }
                    else
                    {
                        messageText = $"<h2>Дедлайн прошел: {goal.Deadline}!<br>" +
                        $"Возможно случится: {goal.Punishment}</h2>";
                    }
                    message.From.Add(MailboxAddress.Parse(_options.Email));

                    message.To.Add(MailboxAddress.Parse(goal.IdUserNavigation.Email));
                    message.Subject = goal.Title;
                    message.Body = new TextPart("html")
                    {
                        Text = messageText
                    };

                    using var client = new SmtpClient();
                    await client.ConnectAsync(_options.SMTP, _options.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(_options.Email, _options.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    Guid id = await sendEmailService.AddAsync(dateTimeManager.RemoveSeconds(DateTime.Now),
                        messageText,
                        goal.Id);
                    SendEmailModel? sendEmail = await sendEmailService.GetByIdAsync(id);
                    if (sendEmail == null) return;
                    await sendEmailService.UpdateAsync(new Core.Models.SendEmailModel(id, sendEmail.Date,
                    sendEmail.Message, true));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
