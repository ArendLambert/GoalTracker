using Core.Models;

namespace GoalTrackerApp.Contracts
{
    public class GoalInputContract
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid IdStatus { get; set; }

        public Guid? IdImportance { get; set; }

        public Guid IdUser { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? Deadline { get; set; }

        public string? Punishment { get; set; }

        public bool AutoImportance { get; set; }
        public IEnumerable<SendEmailModel> SendEmail { get; set; }
    }
}
