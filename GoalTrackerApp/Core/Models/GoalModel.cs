using Core.Interfaces;

namespace Core.Models
{
    public class GoalModel : IModel
    {
        public Guid Id { get; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid IdStatus { get; set; }

        public Guid IdImportance { get; set; }

        public Guid IdUser { get; }

        public DateTime StartDate { get; }

        public DateTime? Deadline { get; set; }

        public string? Punishment { get; set; }

        public bool AutoImportance { get; set; }


        public GoalModel(Guid id, string title, string? description, Guid idStatus, Guid idImportance,
            Guid idUser, DateTime? startDate, DateTime? deadline, string? punishment, bool autoImportance)
        {
            Id = id;
            Title = title;
            Description = description ?? "Задача без подробностей";
            IdStatus = idStatus;
            IdImportance = idImportance;
            IdUser = idUser;
            StartDate = startDate ?? DateTime.Now;
            Deadline = deadline;
            Punishment = punishment;
            AutoImportance = autoImportance;
        }
    }
}
