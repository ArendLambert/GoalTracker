namespace DataAccess.Entities;

public partial class Theme
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string PrimaryColor { get; set; } = null!;

    public string SecondaryColor { get; set; } = null!;

    public string AccentColor { get; set; } = null!;

    public string BackgroundColor { get; set; } = null!;

    public string TextColor { get; set; } = null!;

    public string BorderColor { get; set; } = null!;

    public string ShadowColor { get; set; } = null!;

    public string CardBackground { get; set; } = null!;

    public string ButtonColor { get; set; } = null!;

    public string ButtonTextColor { get; set; } = null!;

    public virtual ICollection<ImportanceTheme> ImportanceThemes { get; set; } = new List<ImportanceTheme>();

    public virtual ICollection<ThemeSet> ThemeSets { get; set; } = new List<ThemeSet>();
}
