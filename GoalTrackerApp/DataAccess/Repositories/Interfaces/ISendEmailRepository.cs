using Core.Models;
using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISendEmailRepository : IRepository<SendEmailModel>
    {
        Task<ICollection<SendEmailModel>> GetAllByIdUserAsync(Guid id);
        Task<ICollection<SendEmail>> GetByTimeAsync(DateTime time);
        Task<ICollection<SendEmail>> GetNotSendedAsync();
    }
}
