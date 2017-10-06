using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportOverviewApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserLog> UserLogs { get; private set; }
        public Themes Theme { get; set; } = Themes.Light;

        /// <summary>
        ///   Themes so far include Light and Dark.
        ///   Light is the normal (default) theme and does not use any extra css files.
        ///   Dark uses the dark-theme.css file, which overrides some of the
        ///   Bootstrap 4 classes and HTML tags.
        /// </summary>
        public enum Themes
        {
            Light, Dark
        }
    }
}
