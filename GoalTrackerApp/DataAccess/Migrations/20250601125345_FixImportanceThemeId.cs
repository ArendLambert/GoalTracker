using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixImportanceThemeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Importance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinDays = table.Column<int>(type: "int", nullable: false),
                    MaxDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Importan__3214EC07E2ADD716", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SendEmail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Sended = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SendEmai__3214EC075BEE7D59", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Status__3214EC072CB3409C", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccentColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BorderColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShadowColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CardBackground = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ButtonColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ButtonTextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Theme__3214EC07E41F5529", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportanceTheme",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdImportance = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdTheme = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackgroundColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Importan__3214EC07BE3FE1CF", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportanceTheme_Importance",
                        column: x => x.IdImportance,
                        principalTable: "Importance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportanceTheme_Theme",
                        column: x => x.IdTheme,
                        principalTable: "Theme",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdStatus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdImportance = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Punishment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoImportance = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Goal__3214EC074EF6A342", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goal_Importance",
                        column: x => x.IdImportance,
                        principalTable: "Importance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Goal_Status",
                        column: x => x.IdStatus,
                        principalTable: "Status",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GoalEmails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdSendEmail = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdGoal = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GoalEmai__3214EC07A465543F", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalEmails_Goal",
                        column: x => x.IdGoal,
                        principalTable: "Goal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GoalEmails_SendEmail",
                        column: x => x.IdSendEmail,
                        principalTable: "SendEmail",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThemeSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IdTheme = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserCreator = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Public = table.Column<bool>(type: "bit", nullable: false),
                    ImportanceThemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThemeSet__3214EC07FFB94BCD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThemeSet_ImportanceTheme_ImportanceThemeId",
                        column: x => x.ImportanceThemeId,
                        principalTable: "ImportanceTheme",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThemeSet_Theme",
                        column: x => x.IdTheme,
                        principalTable: "Theme",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdThemeSet = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__3214EC07434ECC4E", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_ThemeSet",
                        column: x => x.IdThemeSet,
                        principalTable: "ThemeSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goal_IdImportance",
                table: "Goal",
                column: "IdImportance");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_IdStatus",
                table: "Goal",
                column: "IdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_IdUser",
                table: "Goal",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_GoalEmails_IdGoal",
                table: "GoalEmails",
                column: "IdGoal");

            migrationBuilder.CreateIndex(
                name: "IX_GoalEmails_IdSendEmail",
                table: "GoalEmails",
                column: "IdSendEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ImportanceTheme_IdImportance",
                table: "ImportanceTheme",
                column: "IdImportance");

            migrationBuilder.CreateIndex(
                name: "IX_ImportanceTheme_IdTheme",
                table: "ImportanceTheme",
                column: "IdTheme");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeSet_IdTheme",
                table: "ThemeSet",
                column: "IdTheme");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeSet_ImportanceThemeId",
                table: "ThemeSet",
                column: "ImportanceThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeSet_UserCreator",
                table: "ThemeSet",
                column: "UserCreator");

            migrationBuilder.CreateIndex(
                name: "IX_User_IdThemeSet",
                table: "User",
                column: "IdThemeSet");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D10534FAC48322",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_User",
                table: "Goal",
                column: "IdUser",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeSet_UserCreator",
                table: "ThemeSet",
                column: "UserCreator",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportanceTheme_Importance",
                table: "ImportanceTheme");

            migrationBuilder.DropForeignKey(
                name: "FK_ThemeSet_UserCreator",
                table: "ThemeSet");

            migrationBuilder.DropTable(
                name: "GoalEmails");

            migrationBuilder.DropTable(
                name: "Goal");

            migrationBuilder.DropTable(
                name: "SendEmail");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Importance");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ThemeSet");

            migrationBuilder.DropTable(
                name: "ImportanceTheme");

            migrationBuilder.DropTable(
                name: "Theme");
        }
    }
}
