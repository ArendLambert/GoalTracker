using Core.Interfaces;
using Core.Models;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly IGoalEmailRepository _goalEmailRepository;
        private readonly IDateTimeManager _dateTimeManager;
        public SendEmailService(ISendEmailRepository sendEmailRepository, IGoalEmailRepository goalEmailRepository, IDateTimeManager dateTimeManager)
        {
            _sendEmailRepository = sendEmailRepository;
            _goalEmailRepository = goalEmailRepository;
            _dateTimeManager = dateTimeManager;
        }

        public async Task<Guid> AddAsync(DateTime dateTime, string? message, Guid idGoal)
        {
            Guid idSendEmail = Guid.NewGuid();
            DateTime withoutSeconds = _dateTimeManager.RemoveSeconds(dateTime);
            await _sendEmailRepository.AddAsync(new SendEmailModel(idSendEmail, withoutSeconds, message, false));
            await _goalEmailRepository.AddAsync(new GoalEmailModel(Guid.NewGuid(), idSendEmail, idGoal));
            return idSendEmail;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _sendEmailRepository.DeleteAsync(id);
            return !(await _sendEmailRepository.ExistsAsync(id));
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _sendEmailRepository.ExistsAsync(id);
        }

        public Task<ICollection<SendEmailModel>> GetAllAsync()
        {
            return _sendEmailRepository.GetAllAsync();
        }

        public async Task<SendEmailModel?> GetByIdAsync(Guid id)
        {
            return await _sendEmailRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(SendEmailModel entity)
        {
            entity.Date = _dateTimeManager.RemoveSeconds(entity.Date);
            await _sendEmailRepository.UpdateAsync(entity);
        }

        public async Task<ICollection<SendEmailModel>> GetAllByIdUserAsync(Guid id)
        {
            return await _sendEmailRepository.GetAllByIdUserAsync(id);
        }

        public async Task<ICollection<SendEmail>> GetByTimeAsync(DateTime time)
        {
            return await _sendEmailRepository.GetByTimeAsync(time);
        }

        public async Task<ICollection<SendEmail>> GetNotSendedAsync()
        {
            return await _sendEmailRepository.GetNotSendedAsync();
        }
    }
}
