namespace DataAccess.Entities;

public partial class Status
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
}
