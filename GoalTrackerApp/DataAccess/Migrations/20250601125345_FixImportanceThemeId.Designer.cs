﻿// <auto-generated />
using System;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250601125345_FixImportanceThemeId")]
    partial class FixImportanceThemeId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Entities.Goal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool>("AutoImportance")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IdImportance")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdStatus")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Punishment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__Goal__3214EC074EF6A342");

                    b.HasIndex("IdImportance");

                    b.HasIndex("IdStatus");

                    b.HasIndex("IdUser");

                    b.ToTable("Goal", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.GoalEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("IdGoal")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdSendEmail")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK__GoalEmai__3214EC07A465543F");

                    b.HasIndex("IdGoal");

                    b.HasIndex("IdSendEmail");

                    b.ToTable("GoalEmails");
                });

            modelBuilder.Entity("DataAccess.Entities.Importance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("MaxDays")
                        .HasColumnType("int");

                    b.Property<int>("MinDays")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Importan__3214EC07E2ADD716");

                    b.ToTable("Importance", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.ImportanceTheme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("IdImportance")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdTheme")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TextColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__Importan__3214EC07BE3FE1CF");

                    b.HasIndex("IdImportance");

                    b.HasIndex("IdTheme");

                    b.ToTable("ImportanceTheme", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.SendEmail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<bool>("Sended")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__SendEmai__3214EC075BEE7D59");

                    b.ToTable("SendEmail", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Status", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Status__3214EC072CB3409C");

                    b.ToTable("Status", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Theme", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("AccentColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("BorderColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ButtonColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ButtonTextColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CardBackground")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PrimaryColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SecondaryColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ShadowColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TextColor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__Theme__3214EC07E41F5529");

                    b.ToTable("Theme", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.ThemeSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid>("IdTheme")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ImportanceThemeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Public")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UserCreator")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK__ThemeSet__3214EC07FFB94BCD");

                    b.HasIndex("IdTheme");

                    b.HasIndex("ImportanceThemeId");

                    b.HasIndex("UserCreator");

                    b.ToTable("ThemeSet", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("IdThemeSet")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK__User__3214EC07434ECC4E");

                    b.HasIndex("IdThemeSet");

                    b.HasIndex(new[] { "Email" }, "UQ__User__A9D10534FAC48322")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Goal", b =>
                {
                    b.HasOne("DataAccess.Entities.Importance", "IdImportanceNavigation")
                        .WithMany("Goals")
                        .HasForeignKey("IdImportance")
                        .IsRequired()
                        .HasConstraintName("FK_Goal_Importance");

                    b.HasOne("DataAccess.Entities.Status", "IdStatusNavigation")
                        .WithMany("Goals")
                        .HasForeignKey("IdStatus")
                        .IsRequired()
                        .HasConstraintName("FK_Goal_Status");

                    b.HasOne("DataAccess.Entities.User", "IdUserNavigation")
                        .WithMany("Goals")
                        .HasForeignKey("IdUser")
                        .IsRequired()
                        .HasConstraintName("FK_Goal_User");

                    b.Navigation("IdImportanceNavigation");

                    b.Navigation("IdStatusNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.GoalEmail", b =>
                {
                    b.HasOne("DataAccess.Entities.Goal", "IdGoalNavigation")
                        .WithMany("GoalEmails")
                        .HasForeignKey("IdGoal")
                        .IsRequired()
                        .HasConstraintName("FK_GoalEmails_Goal");

                    b.HasOne("DataAccess.Entities.SendEmail", "IdSendEmailNavigation")
                        .WithMany("GoalEmails")
                        .HasForeignKey("IdSendEmail")
                        .IsRequired()
                        .HasConstraintName("FK_GoalEmails_SendEmail");

                    b.Navigation("IdGoalNavigation");

                    b.Navigation("IdSendEmailNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.ImportanceTheme", b =>
                {
                    b.HasOne("DataAccess.Entities.Importance", "IdImportanceNavigation")
                        .WithMany("ImportanceThemes")
                        .HasForeignKey("IdImportance")
                        .IsRequired()
                        .HasConstraintName("FK_ImportanceTheme_Importance");

                    b.HasOne("DataAccess.Entities.Theme", "IdThemeNavigation")
                        .WithMany("ImportanceThemes")
                        .HasForeignKey("IdTheme")
                        .IsRequired()
                        .HasConstraintName("FK_ImportanceTheme_Theme");

                    b.Navigation("IdImportanceNavigation");

                    b.Navigation("IdThemeNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.ThemeSet", b =>
                {
                    b.HasOne("DataAccess.Entities.Theme", "IdThemeNavigation")
                        .WithMany("ThemeSets")
                        .HasForeignKey("IdTheme")
                        .IsRequired()
                        .HasConstraintName("FK_ThemeSet_Theme");

                    b.HasOne("DataAccess.Entities.ImportanceTheme", null)
                        .WithMany("ThemeSets")
                        .HasForeignKey("ImportanceThemeId");

                    b.HasOne("DataAccess.Entities.User", "UserCreatorNavigation")
                        .WithMany("ThemeSets")
                        .HasForeignKey("UserCreator")
                        .HasConstraintName("FK_ThemeSet_UserCreator");

                    b.Navigation("IdThemeNavigation");

                    b.Navigation("UserCreatorNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.HasOne("DataAccess.Entities.ThemeSet", "IdThemeSetNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdThemeSet")
                        .HasConstraintName("FK_User_ThemeSet");

                    b.Navigation("IdThemeSetNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Goal", b =>
                {
                    b.Navigation("GoalEmails");
                });

            modelBuilder.Entity("DataAccess.Entities.Importance", b =>
                {
                    b.Navigation("Goals");

                    b.Navigation("ImportanceThemes");
                });

            modelBuilder.Entity("DataAccess.Entities.ImportanceTheme", b =>
                {
                    b.Navigation("ThemeSets");
                });

            modelBuilder.Entity("DataAccess.Entities.SendEmail", b =>
                {
                    b.Navigation("GoalEmails");
                });

            modelBuilder.Entity("DataAccess.Entities.Status", b =>
                {
                    b.Navigation("Goals");
                });

            modelBuilder.Entity("DataAccess.Entities.Theme", b =>
                {
                    b.Navigation("ImportanceThemes");

                    b.Navigation("ThemeSets");
                });

            modelBuilder.Entity("DataAccess.Entities.ThemeSet", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.Navigation("Goals");

                    b.Navigation("ThemeSets");
                });
#pragma warning restore 612, 618
        }
    }
}
