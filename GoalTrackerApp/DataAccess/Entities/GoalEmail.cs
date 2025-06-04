namespace DataAccess.Entities;

public partial class GoalEmail
{
    public Guid Id { get; set; }

    public Guid IdSendEmail { get; set; }

    public Guid IdGoal { get; set; }

    public virtual Goal IdGoalNavigation { get; set; } = null!;

    public virtual SendEmail IdSendEmailNavigation { get; set; } = null!;
}
