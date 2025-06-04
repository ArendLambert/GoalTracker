using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class ImportanceThemeModel : IModel
    {
        public Guid Id { get; }

        public Guid IdImportance { get; set; }

        public Guid IdTheme { get; set; }

        public string BackgroundColor { get; set; } = null!;

        public string TextColor { get; set; } = null!;


        public ImportanceThemeModel(Guid id, Guid idImportance, Guid idTheme, string backgroundColor, string textColor)
        {
            Id = id;
            IdImportance = idImportance;
            IdTheme = idTheme;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
        }
    }
}
