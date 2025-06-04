using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<GoalEmail> GoalEmails { get; set; }

    public virtual DbSet<Importance> Importances { get; set; }

    public virtual DbSet<ImportanceTheme> ImportanceThemes { get; set; }

    public virtual DbSet<SendEmail> SendEmails { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Theme> Themes { get; set; }

    public virtual DbSet<ThemeSet> ThemeSets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GoalTrecker;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Goal__3214EC074EF6A342");

            entity.ToTable("Goal");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.IdImportanceNavigation).WithMany(p => p.Goals)
                .HasForeignKey(d => d.IdImportance)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_Importance");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Goals)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_Status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Goals)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Goal_User");
        });

        modelBuilder.Entity<GoalEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GoalEmai__3214EC07A465543F");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdGoalNavigation).WithMany(p => p.GoalEmails)
                .HasForeignKey(d => d.IdGoal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoalEmails_Goal");

            entity.HasOne(d => d.IdSendEmailNavigation).WithMany(p => p.GoalEmails)
                .HasForeignKey(d => d.IdSendEmail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GoalEmails_SendEmail");
        });

        modelBuilder.Entity<Importance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Importan__3214EC07E2ADD716");

            entity.ToTable("Importance");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<ImportanceTheme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Importan__3214EC07BE3FE1CF");

            entity.ToTable("ImportanceTheme");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BackgroundColor).HasMaxLength(20);
            entity.Property(e => e.TextColor).HasMaxLength(20);

            entity.HasOne(d => d.IdImportanceNavigation).WithMany(p => p.ImportanceThemes)
                .HasForeignKey(d => d.IdImportance)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportanceTheme_Importance");

            entity.HasOne(d => d.IdThemeNavigation).WithMany(p => p.ImportanceThemes)
                .HasForeignKey(d => d.IdTheme)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImportanceTheme_Theme");
        });

        modelBuilder.Entity<SendEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SendEmai__3214EC075BEE7D59");

            entity.ToTable("SendEmail");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.Sended).HasColumnType("bit");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC072CB3409C");

            entity.ToTable("Status");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Theme__3214EC07E41F5529");

            entity.ToTable("Theme");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccentColor).HasMaxLength(20);
            entity.Property(e => e.BackgroundColor).HasMaxLength(20);
            entity.Property(e => e.BorderColor).HasMaxLength(20);
            entity.Property(e => e.ButtonColor).HasMaxLength(20);
            entity.Property(e => e.ButtonTextColor).HasMaxLength(20);
            entity.Property(e => e.CardBackground).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PrimaryColor).HasMaxLength(20);
            entity.Property(e => e.SecondaryColor).HasMaxLength(20);
            entity.Property(e => e.ShadowColor).HasMaxLength(20);
            entity.Property(e => e.TextColor).HasMaxLength(20);
        });

        modelBuilder.Entity<ThemeSet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThemeSet__3214EC07FFB94BCD");

            entity.ToTable("ThemeSet");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdThemeNavigation).WithMany(p => p.ThemeSets)
                .HasForeignKey(d => d.IdTheme)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThemeSet_Theme");

            entity.HasOne(d => d.UserCreatorNavigation).WithMany(p => p.ThemeSets)
                .HasForeignKey(d => d.UserCreator)
                .HasConstraintName("FK_ThemeSet_UserCreator");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07434ECC4E");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534FAC48322").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.HasOne(d => d.IdThemeSetNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdThemeSet)
                .HasConstraintName("FK_User_ThemeSet");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
