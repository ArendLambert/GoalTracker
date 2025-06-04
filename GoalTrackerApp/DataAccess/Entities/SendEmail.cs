namespace DataAccess.Entities;

public partial class SendEmail
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public string? Message { get; set; }
    public bool Sended { get; set; }

    public virtual ICollection<GoalEmail> GoalEmails { get; set; } = new List<GoalEmail>();
}
