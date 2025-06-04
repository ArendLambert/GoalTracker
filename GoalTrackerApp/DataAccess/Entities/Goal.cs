namespace DataAccess.Entities;

public partial class Goal
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid IdStatus { get; set; }

    public Guid IdImportance { get; set; }

    public Guid IdUser { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? Deadline { get; set; }

    public string? Punishment { get; set; }

    public bool AutoImportance { get; set; }

    public virtual ICollection<GoalEmail> GoalEmails { get; set; } = new List<GoalEmail>();

    public virtual Importance IdImportanceNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    //public virtual ICollection<SendEmail> SendEmails { get; set; } = new List<SendEmail>();
}
