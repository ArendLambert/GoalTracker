using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core
{
    public class DateTimeManager : IDateTimeManager
    {
        public DateTime RemoveSeconds(DateTime dateTimeOriginal)
        {
            return new DateTime(
                dateTimeOriginal.Year,
                dateTimeOriginal.Month,
                dateTimeOriginal.Day,
                dateTimeOriginal.Hour,
                dateTimeOriginal.Minute,
                0
            );
        }
    }
}
