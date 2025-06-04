using Core.Interfaces;

namespace Core.Models
{
    public class SendEmailModel : IModel
    {
        public Guid Id { get; }

        public DateTime Date { get; set; }

        public string? Message { get; set; }
        public bool Sended { get; set; }

        public SendEmailModel(Guid id, DateTime date, string? message, bool sended)
        {
            Id = id;
            Date = date;
            Message = message;
            Sended = sended;
        }
    }
}
