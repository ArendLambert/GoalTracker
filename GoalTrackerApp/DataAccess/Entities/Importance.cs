namespace DataAccess.Entities;

public partial class Importance
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public int MinDays { get; set; }

    public int MaxDays { get; set; }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<ImportanceTheme> ImportanceThemes { get; set; } = new List<ImportanceTheme>();
}
