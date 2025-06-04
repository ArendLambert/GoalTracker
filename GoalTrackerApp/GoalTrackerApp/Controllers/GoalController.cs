using System;
using Core;
using Core.Interfaces;
using Core.Models;
using DataAccess.Services.Interfaces;
using GoalTrackerApp.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalTrackerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly ISendEmailService _sendEmailService;
        private readonly IGoalEmailService _goalEmailService;
        private readonly IImportanceService _importanceService;
        private readonly IDateTimeManager _dateTimeManager;
        public GoalController(IGoalService goalService, ISendEmailService sendEmailService, 
            IGoalEmailService goalEmailService, IDateTimeManager dateTimeManager, IImportanceService importanceService)
        {
            _goalService = goalService;
            _sendEmailService = sendEmailService;
            _goalEmailService = goalEmailService;
            _dateTimeManager = dateTimeManager;
            _importanceService = importanceService;
        }

        [HttpGet]
        [Route("{idUser:guid}/{id:guid}")]
        [Authorize]
        public async Task<ActionResult<GoalContract>> GetByIdAsync(Guid idUser, Guid id)
        {
            GoalModel? goal = await _goalService.GetByIdAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            if (goal.IdUser != idUser)
            {
                return Forbid();
            }
            List<GoalEmailModel> goalEmails = (List<GoalEmailModel>)await _goalEmailService.GetByIdGoalAsync(goal.Id);
            List<SendEmailModel> sendEmails = new List<SendEmailModel>();
            foreach (GoalEmailModel email in goalEmails)
            {
                sendEmails.Add(await _sendEmailService.GetByIdAsync(email.IdSendEmail));
            }
            return Ok(new GoalContract
            {
                Id = goal.Id,
                IdImportance = goal.IdImportance,
                IdStatus = goal.IdStatus,
                AutoImportance = goal.AutoImportance,
                Deadline = goal.Deadline,
                Description = goal.Description,
                Punishment = goal.Punishment,
                Title = goal.Title,
                GoalEmail = goalEmails,
                SendEmail = sendEmails,
                IdUser = goal.IdUser,
                StartDate = goal.StartDate
            });
        }

        [HttpGet]
        [Route("allforuser/{idUser:guid}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GoalContract>>> GetAllByUserIdAsync(Guid idUser)
        {
            List<GoalContract> goalContracts = new List<GoalContract>();
            List<GoalModel> goals = (List<GoalModel>)await _goalService.GetByUserIdAsync(idUser);
            //if (goals == null)
            //{
            //    return NotFound();
            //} 
            if (goals.Any(x => x.IdUser != idUser))
            {
                return Forbid();
            }
            foreach (GoalModel goal in goals)
            {
                List<GoalEmailModel> goalEmails = (List<GoalEmailModel>)await _goalEmailService.GetByIdGoalAsync(goal.Id);
                List<SendEmailModel> sendEmails = new List<SendEmailModel>();
                foreach (GoalEmailModel email in goalEmails)
                {
                    sendEmails.Add(await _sendEmailService.GetByIdAsync(email.IdSendEmail));
                }
                goalContracts.Add(new GoalContract
                {
                    Id = goal.Id,
                    IdImportance = goal.IdImportance,
                    IdStatus = goal.IdStatus,
                    AutoImportance = goal.AutoImportance,
                    Deadline = goal.Deadline,
                    Description = goal.Description,
                    Punishment = goal.Punishment,
                    Title = goal.Title,
                    GoalEmail = goalEmails,
                    SendEmail = sendEmails,
                    IdUser = goal.IdUser,
                    StartDate = goal.StartDate
                });
            }
            return Ok(goalContracts);
        }

        [HttpPost]
        [Route("add")]
        [Authorize]
        public async Task<ActionResult> AddAsync([FromBody] GoalInputContract goalContract)
        {
            try
            {
                if(goalContract.AutoImportance == true)
                {
                    IEnumerable<ImportanceModel> importances = await _importanceService.GetAllAsync();
                    if (goalContract.Deadline == null)
                    {
                        goalContract.IdImportance = importances.First(x => x.MaxDays == 365).Id;
                    }
                    else
                    {
                        TimeSpan difference = ((DateTime)goalContract.Deadline) - goalContract.StartDate;
                        int duration = Math.Abs(difference.Days);
                        ImportanceModel? importance = importances.FirstOrDefault(x =>
                        x.MinDays <= duration
                        && x.MaxDays >= duration);
                        if (importance == null)
                        {
                            importance = importances.First(x => x.MaxDays == 365);
                        }

                        goalContract.IdImportance = importance.Id;
                    }
                    
                }
                Guid idGoal = await _goalService.AddAsync(goalContract.Title, goalContract.Description, goalContract.IdStatus,
                            (Guid)goalContract.IdImportance, goalContract.IdUser, goalContract.StartDate, goalContract.Deadline,
                            goalContract.Punishment, goalContract.AutoImportance);
                foreach (SendEmailModel email in goalContract.SendEmail)
                {
                    if (email.Date > goalContract.Deadline)
                    {
                        email.Date = goalContract.Deadline ?? _dateTimeManager.RemoveSeconds(DateTime.Now);
                    }
                    if (email.Date < goalContract.StartDate)
                    {
                        email.Date = goalContract.StartDate.AddHours(1);
                    }
                    await _sendEmailService.AddAsync(email.Date, email.Message, idGoal);
                }
                goalContract.Id = idGoal;
                return Ok(goalContract);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("update/{idUser:guid}")]
        [Authorize]
        public async Task<ActionResult> UpdateAsync([FromBody] GoalInputContract goalContract, Guid idUser)
        {
            try
            {
                GoalModel? goal = await _goalService.GetByIdAsync(goalContract.Id);
                if (goal == null)
                {
                    return NotFound();
                }
                if (goal.IdUser != idUser)
                {
                    return Forbid();
                }

                if (goalContract.AutoImportance == true)
                {
                    IEnumerable<ImportanceModel> importances = await _importanceService.GetAllAsync();
                    if (goalContract.Deadline == null)
                    {
                        goalContract.IdImportance = importances.First(x => x.MaxDays == 365).Id;
                    }
                    else
                    {
                        TimeSpan difference = ((DateTime)goalContract.Deadline) - goalContract.StartDate;
                        int duration = Math.Abs(difference.Days);
                        ImportanceModel? importance = importances.FirstOrDefault(x =>
                        x.MinDays <= duration
                        && x.MaxDays >= duration);
                        if(importance == null)
                        {
                            importance = importances.First(x => x.MaxDays == 365);
                        }
                        goalContract.IdImportance = importance.Id;
                        
                    }

                }

                await _goalService.UpdateAsync(new GoalModel(goalContract.Id, goalContract.Title, goalContract.Description,
                    goalContract.IdStatus, (Guid)goalContract.IdImportance, goalContract.IdUser, goalContract.StartDate,
                    goalContract.Deadline, goalContract.Punishment, goalContract.AutoImportance));
                foreach (SendEmailModel email in goalContract.SendEmail)
                {
                    if (email.Date > goalContract.Deadline)
                    {
                        email.Date = goalContract.Deadline ?? _dateTimeManager.RemoveSeconds(DateTime.Now);
                    }
                    if (email.Date < goalContract.StartDate)
                    {
                        email.Date = goalContract.StartDate.AddHours(1);
                    }
                    if (await _sendEmailService.ExistsAsync(email.Id))
                    {
                        List<GoalEmailModel> goalEmails = (List<GoalEmailModel>)await _goalEmailService.GetByIdGoalAsync(goalContract.Id);
                        goalEmails.RemoveAll(x => x.IdSendEmail == email.Id);
                        if (goalEmails.Count > 0)
                        {
                            foreach (GoalEmailModel goalEmail in goalEmails)
                            {
                                await _goalEmailService.DeleteAsync(goalEmail.Id);
                                await _sendEmailService.DeleteAsync(goalEmail.IdSendEmail);
                            }                            
                        }
                        await _sendEmailService.UpdateAsync(email);
                    }
                    else
                    {
                        Guid id = await _sendEmailService.AddAsync(email.Date, email.Message, goalContract.Id);
                        //await _goalEmailService.AddAsync(goalContract.Id, id);
                    }
                }
                return Ok(goalContract);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                return BadRequest(new { message = "Произошла внутренняя ошибка сервера", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{idUser:guid}/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync(Guid idUser, Guid id)
        {
            try
            {
                GoalModel? goal = await _goalService.GetByIdAsync(id);
                if (goal == null)
                {
                    return NotFound();
                }
                if (goal.IdUser != idUser)
                {
                    return Forbid();
                }
                IEnumerable<GoalEmailModel> goalEmails = await _goalEmailService.GetByIdGoalAsync(id);
                foreach (GoalEmailModel email in goalEmails)
                {
                    await _goalEmailService.DeleteAsync(email.Id);
                }
                foreach (GoalEmailModel email in goalEmails)
                {
                    SendEmailModel? sendEmail = await _sendEmailService.GetByIdAsync(email.IdSendEmail);
                    if (sendEmail == null)
                    {
                        continue;
                    }
                    await _sendEmailService.DeleteAsync(sendEmail.Id);
                }
                await _goalService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Произошла внутренняя ошибка сервера", error = ex.Message });
            }
        }
    }
}
