using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class UserLog
    {
        public int ID {get; set;}
        public string UserID { get; set; }
        public string Message { get; set; }
        public DateTime? TimeStamp { get; set; }

        public string ToString(string User)
        {
            return $"{TimeStamp.Value.ToString()}:  {Message}";
        }
    }
}
