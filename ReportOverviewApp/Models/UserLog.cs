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
        public int ID {get; private set;}
        public int ReportID { get; private set; }
        public string UserID { get; private set; }
        public string Message { get; private set; }
        public string Changes { get; private set; }
        public DateTime? TimeStamp { get; private set; }

        public UserLog() { }
        public UserLog(string userId, int reportId, string message, DateTime? timeStamp, string changes = null)
        {
            UserID = userId;
            Message = message;
            Changes = changes;
            TimeStamp = timeStamp;
            ReportID = reportId;
        }

        public override string ToString()
        {
            return $"{TimeStamp.Value.ToString()}:  {Message}";
        }
    }
}
