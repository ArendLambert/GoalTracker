using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class UserModel : IModel
    {
        public Guid Id { get; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public Guid? IdThemeSet { get; set; }


        public UserModel(Guid id, string email, string password, Guid? idThemeSet = null)
        {
            Id = id;
            Email = email;
            Password = password;
            IdThemeSet = idThemeSet;
        }
    }
}
