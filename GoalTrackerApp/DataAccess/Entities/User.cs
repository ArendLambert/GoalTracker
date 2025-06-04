namespace DataAccess.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid? IdThemeSet { get; set; }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ThemeSet? IdThemeSetNavigation { get; set; }

    public virtual ICollection<ThemeSet> ThemeSets { get; set; } = new List<ThemeSet>();
}
