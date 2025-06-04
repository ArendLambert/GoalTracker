using Core.Interfaces;
using Core.Models;
using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class SendEmailRepository : ISendEmailRepository
    {
        public AppDbContext Context {  get; }
        private readonly IDateTimeManager _dateTimeManager;
        public SendEmailRepository(AppDbContext context, IDateTimeManager dateTimeManager)
        {
            Context = context;
            _dateTimeManager = dateTimeManager;
        }

        public async Task AddAsync(SendEmailModel entity)
        {
            await Context.SendEmails.AddAsync(new SendEmail 
            {
                Id = entity.Id,
                Date = entity.Date,
                Message = entity.Message
            });
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await Context.SendEmails.Where(x => x.Id == id).ExecuteDeleteAsync();
            await Context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await Context.SendEmails.FindAsync(id) != null;
        }

        public async Task<ICollection<SendEmailModel>> GetAllAsync()
        {
            return await Context.SendEmails.AsNoTracking()
                .Select(x => new SendEmailModel(x.Id, x.Date, x.Message, false))
                .ToListAsync();
        }

        public async Task<SendEmailModel?> GetByIdAsync(Guid id)
        {
            SendEmail? sendEmail = await Context.SendEmails.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (sendEmail == null)
            {
                return null;
            }
            return new SendEmailModel(sendEmail.Id, sendEmail.Date, sendEmail.Message, false);
        }

        public async Task UpdateAsync(SendEmailModel entity)
        {
            SendEmail? sendEmail = await Context.SendEmails.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (sendEmail == null)
            {
                throw new InvalidOperationException("Entity not found");
            }
            sendEmail.Date = entity.Date;
            sendEmail.Message = entity.Message;
            sendEmail.Sended = entity.Sended;
            await Context.SaveChangesAsync();
        }

        public async Task<ICollection<SendEmailModel>> GetAllByIdUserAsync(Guid id)
        {
            ICollection<Goal> goals = await Context.Goals.AsNoTracking().Where(x => x.IdUser == id).Include(x => x.GoalEmails).ThenInclude(x => x.IdSendEmailNavigation)
                .ToListAsync();
            return goals.SelectMany(x => x.GoalEmails.Select(x => x.IdSendEmailNavigation))
                .Select(x => new SendEmailModel(x.Id, x.Date, x.Message, false)).ToList();
            //throw new NotImplementedException("This method is not implemented yet.");
        }

        public async Task<ICollection<SendEmail>> GetByTimeAsync(DateTime time)
        {
            DateTime inputDate = _dateTimeManager.RemoveSeconds(time);
            return await Context.SendEmails.AsNoTracking().Where(x => x.Date == inputDate)
                .Include(x => x.GoalEmails)
                .ThenInclude(x => x.IdGoalNavigation)
                .ThenInclude(x => x.IdUserNavigation)
                //.Select(x => new SendEmailModel(x.Id, x.Date, x.Message, null))
                .ToListAsync();
        }

        public async Task<ICollection<SendEmail>> GetNotSendedAsync()
        {
            DateTime timeNow = _dateTimeManager.RemoveSeconds(DateTime.Now);
            return await Context.SendEmails.AsNoTracking().Where(x => x.Sended == false && x.Date < timeNow)
                .Include(x => x.GoalEmails)
                .ThenInclude(x => x.IdGoalNavigation)
                .ThenInclude(x => x.IdUserNavigation)
                //.Select(x => new SendEmailModel(x.Id, x.Date, x.Message, null))
                .ToListAsync();
        }
    }
}
