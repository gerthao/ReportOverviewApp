using ReportOverviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Helpers
{
    public class UserLogFactory : CustomFactory<UserLog>
    {
        public override UserLog Build()
        {
            return new UserLog();
        }
        public UserLog Build(string _userId, string _message, DateTime? _timeStamp = null)
        {
            UserLog item = new UserLog()
            {
                UserID = _userId,
                Message = _message,
                TimeStamp = _timeStamp == null ? new DateTime() : _timeStamp
            };
            return item;
        }
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
