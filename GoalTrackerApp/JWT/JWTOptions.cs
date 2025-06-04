using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT
{
    public class JWTOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = "GoalTrackerApp";
        public int ExpiredHours { get; set; }
    }
}
