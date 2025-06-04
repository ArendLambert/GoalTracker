using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Models
{
    public class ImportanceModel : IModel
    {
        public Guid Id { get; }

        public string Title { get; set; } = null!;

        public int MinDays { get; set; }

        public int MaxDays { get; set; }

        public ImportanceModel(Guid id, string title, int minDays, int maxDays)
        {
            Id = id;
            Title = title;
            MinDays = minDays;
            MaxDays = maxDays;
        }
    }
}
