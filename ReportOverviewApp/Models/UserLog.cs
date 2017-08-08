using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class UserLog
    {
        public int ID {get; private set;}
        public string UserID { get; private set; }
        public string Message { get; private set; }
        public string Changes { get; private set; }
        public DateTime? TimeStamp { get; private set; }

        public UserLog() { }
        public UserLog(string _userID, string _message, string _changes=null, DateTime? _timeStamp)
        {
            UserID = _userID;
            Message = _message;
            Changes = _changes;
            TimeStamp = _timeStamp;
        }

        public override string ToString()
        {
            return $"{TimeStamp.Value.ToString()}:  {Message}";
        }
    }
}
