using ReportOverviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    public class UserLogFactory : Factory<UserLog>
    {
        
        public override UserLog Build() => new UserLog();
        public UserLog Build(string _userId, int _reportId, string _message, string _changes = null, DateTime? _timeStamp = null) 
            => new UserLog(userId: _userId, reportId: _reportId, message: _message, changes: _changes, timeStamp: _timeStamp == null ? DateTime.Now : _timeStamp);
    }
}
