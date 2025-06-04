using Core.Models;
using DataAccess.Entities;

namespace DataAccess.Services.Interfaces
{
    public interface ISendEmailService
    {
        Task<Guid> AddAsync(DateTime dateTime, string? message, Guid idGoal);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<ICollection<SendEmailModel>> GetAllAsync();
        Task<ICollection<SendEmailModel>> GetAllByIdUserAsync(Guid id);
        Task<SendEmailModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(SendEmailModel entity);
        Task<ICollection<SendEmail>> GetByTimeAsync(DateTime time);
        Task<ICollection<SendEmail>> GetNotSendedAsync();
    }
}