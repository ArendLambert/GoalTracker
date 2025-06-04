using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class ThemeModel : IModel
    {
        public Guid Id { get; }

        public string Name { get; set; } = null!;

        public string PrimaryColor { get; set; } = null!;

        public string SecondaryColor { get; set; } = null!;

        public string AccentColor { get; set; } = null!;

        public string BackgroundColor { get; set; } = null!;

        public string TextColor { get; set; } = null!;

        public string BorderColor { get; set; } = null!;

        public string ShadowColor { get; set; } = null!;

        public string CardBackground { get; set; } = null!;

        public string ButtonColor { get; set; } = null!;

        public string ButtonTextColor { get; set; } = null!;

        public ThemeModel(Guid id, string name, string primaryColor, string secondaryColor, string accentColor,
            string backgroundColor, string textColor, string borderColor, string shadowColor, string cardBackground,
            string buttonColor, string buttonTextColor)
        {
            Id = id;
            Name = name;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            AccentColor = accentColor;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            BorderColor = borderColor;
            ShadowColor = shadowColor;
            CardBackground = cardBackground;
            ButtonColor = buttonColor;
            ButtonTextColor = buttonTextColor;
        }
    }
}
