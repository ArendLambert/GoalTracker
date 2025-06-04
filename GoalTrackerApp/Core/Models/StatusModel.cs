using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class StatusModel : IModel
    {
        public Guid Id { get; }

        public string Title { get; set; } = null!;

        public StatusModel(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
