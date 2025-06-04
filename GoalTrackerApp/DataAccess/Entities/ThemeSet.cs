namespace DataAccess.Entities;

public partial class ThemeSet
{
    public Guid Id { get; set; }

    public Guid IdTheme { get; set; }

    public Guid? UserCreator { get; set; }

    public bool Public { get; set; }

    public virtual Theme IdThemeNavigation { get; set; } = null!;

    public virtual User? UserCreatorNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
