using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace ReportOverviewApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserLog> UserLogs { get; private set; }
    }
}
