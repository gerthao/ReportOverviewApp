using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    /// <summary>
    /// Class describes what action has happened to a report by a user.
    /// Changes field not yet implemented.
    /// </summary>
    public class UserLog
    {
        public int Id {get; private set;}
        public ApplicationUser User { get; private set; }
        public string UserId { get; private set; }
        public string Message { get; private set; }
        public string Notes { get; private set; }
        public DateTime? TimeStamp { get; private set; }

        public UserLog() { }
        public UserLog(string userId, string message, DateTime? timeStamp, string notes = null)
        {
            UserId = userId;
            Message = message;
            Notes = notes;
            TimeStamp = timeStamp;
        }

        public override string ToString()
        {
            return $"{TimeStamp.Value.ToString()}:  {Message}";
        }
    }
}
