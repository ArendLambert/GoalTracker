namespace DataAccess.Entities;

public partial class ImportanceTheme
{
    public Guid Id { get; set; }

    public Guid IdImportance { get; set; }

    public Guid IdTheme { get; set; }

    public string BackgroundColor { get; set; } = null!;

    public string TextColor { get; set; } = null!;

    public virtual Importance IdImportanceNavigation { get; set; } = null!;

    public virtual Theme IdThemeNavigation { get; set; } = null!;

    public virtual ICollection<ThemeSet> ThemeSets { get; set; } = new List<ThemeSet>();
}
