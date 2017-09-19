using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ReportOverviewApp.Models.WidgetModels;

namespace ReportOverviewApp.Models.HomeViewModels
{
    /// <summary>
    ///  This ViewModel class holds Report, Widget, and User data
    /// </summary>
    public class HomeViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        public Dictionary<string, string> Users { get; set; }
        public IEnumerable<UserLog> UserLogs { get; set; }

        /// <summary>
        /// Returns a username based on the userId given.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Returns a username as a string.  If not found, then returns "No user".
        /// </returns>
        public string GetUserName(string userId)
        {
            foreach(var usr in Users)
            {
                if (usr.Key.Equals(userId)) return usr.Value;
            }
            return "No user";
        }
    }
}
