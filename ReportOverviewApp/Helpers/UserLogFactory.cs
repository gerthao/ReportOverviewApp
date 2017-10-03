using ReportOverviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    public class UserLogFactory : Factory<UserLog>
    {
        
        public UserLog Build() => new UserLog();
        public UserLog Build(string _userId, string _message, string _notes = null, DateTime? _timeStamp = null) 
            => new UserLog(userId: _userId, message: _message, notes: _notes, timeStamp: _timeStamp == null ? DateTime.Now : _timeStamp);
    }
}
