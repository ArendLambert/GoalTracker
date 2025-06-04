using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class ThemeSetModel : IModel
    {
        public Guid Id { get; }

        public Guid IdTheme { get; set; }

        public Guid? IdUserCreator { get; set; }

        public bool Public { get; set; }


        public ThemeSetModel(Guid id, Guid idTheme, Guid? idUserCreator, bool @public)
        {
            Id = id;
            IdTheme = idTheme;
            IdUserCreator = idUserCreator;
            Public = @public;
        }
    }
}
