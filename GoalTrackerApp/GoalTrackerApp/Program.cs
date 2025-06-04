using Abstractions;
using Core;
using Core.Interfaces;
using Core.Models;
using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services;
using DataAccess.Services.Interfaces;
using EmailSender;
using GoalTrackerApp.Extensions;
using JWT;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>();

        builder.Services.AddAuthorization();
        builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWTOptions"));
        builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGoalEmailRepository, GoalEmailRepository>();
        builder.Services.AddScoped<ISendEmailRepository, SendEmailRepository>();
        builder.Services.AddScoped<IThemeSetRepository, ThemeSetRepository>();
        builder.Services.AddScoped<IThemeReposirory, ThemeRepository>();
        builder.Services.AddScoped<IImportanceThemeRepository, ImportanceThemeRepository>();
        builder.Services.AddScoped<IRepository<ImportanceModel>, ImportanceModelrepository>();
        builder.Services.AddScoped<IGoalRepository, GoalRepository>();
        builder.Services.AddScoped<IRepository<StatusModel>, StatusRepository>();

        builder.Services.AddScoped<ISendEmailService, SendEmailService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IThemeSetService, ThemeSetService>();
        builder.Services.AddScoped<IThemeService, ThemeService>();
        builder.Services.AddScoped<IImportanceThemeService, ImportanceThemeService>();
        builder.Services.AddScoped<IImportanceService, ImportanceService>();
        builder.Services.AddScoped<IGoalService, GoalService>();
        builder.Services.AddScoped<IGoalEmailService, GoalEmailService>();
        builder.Services.AddScoped<IStatusService, StatusService>();

        builder.Services.AddScoped<IJWTProvider, JWTProvider>();
        builder.Services.AddScoped<IDateTimeManager, DateTimeManager>();
        builder.Services.AddSingleton<EmailSender.EmailSender>();

        builder.Services.AddApiAuthentifications(builder.Configuration);

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        EmailSender.EmailSender emailSender = app.Services.GetRequiredService<EmailSender.EmailSender>();

        app.UseCors(x =>
        {
            x.WithOrigins("http://localhost:5173")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}