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
        public UserLog Build(string _userId, string _message, string _changes = null, DateTime? _timeStamp = null) 
            => new UserLog(userId: _userId, message: _message, changes: _changes, timeStamp: _timeStamp == null ? DateTime.Now : _timeStamp);
        public override bool HasProduct(UserLog item)
        {
            foreach(UserLog product in Products)
            {
                if (product.Equals(item)) return true;
            }
            return false;
        }
        protected override UserLog Retrieve(UserLog item)
        {
            if (HasProduct(item)) return item;
            else return null;
        }
        public override void Register(UserLog item) 
        {
            if (!HasProduct(item))
            {
                Products.Add(item);
            }
        }
    }
}
