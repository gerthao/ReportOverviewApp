using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.WidgetModels;

namespace ReportOverviewApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ReportDeadline> ReportDeadlines { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Report>().ToTable("Report");
            builder.Entity<UserLog>().HasOne(ul => ul.User).WithMany(usr => usr.UserLogs).HasForeignKey(ul => ul.UserId);
            builder.Entity<UserLog>().ToTable("UserLog");
            builder.Entity<ReportDeadline>().HasOne(rd => rd.Report).WithMany(rpt => rpt.Deadlines).HasForeignKey(rd => rd.ReportId);
            builder.Entity<ReportDeadline>().ToTable("ReportDeadlines");
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        
    }
}
