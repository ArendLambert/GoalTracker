using Core.Interfaces;

namespace Core.Models
{
    public class GoalEmailModel : IModel
    {
        public Guid Id { get; }

        public Guid IdSendEmail { get; set; }

        public Guid IdGoal { get; set; }


        public GoalEmailModel(Guid id, Guid idSendEmail, Guid idGoal)
        {
            Id = id;
            IdSendEmail = idSendEmail;
            IdGoal = idGoal;
        }
    }
}
