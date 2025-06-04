using Core.Models;

namespace GoalTrackerApp.Contracts
{
    public class ThemeSetContract
    {
        public Guid Id { get; set; }

        public Guid? IdUserCreator { get; set; }

        public bool Public { get; set; }
        public ThemeModel? Theme { get; set; }
        public required IEnumerable<ImportanceThemeModel> ImportanceThemes { get; set; }
        public Guid? RequestUserId { get; set; }
    }
}
