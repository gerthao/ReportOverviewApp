using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<BusinessContact> BusinessContacts { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ReportPlanMap> ReportPlanMapping { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<ReportDeadline> ReportDeadlines { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Report>().ToTable("Reports");

            builder.Entity<ReportPlanMap>().HasOne(rpm => rpm.Report).WithMany(r => r.ReportPlanMapping).HasForeignKey(rpm => rpm.ReportId);
            builder.Entity<ReportPlanMap>().HasOne(rpm => rpm.Plan).WithMany(p => p.ReportPlanMapping).HasForeignKey(rpm => rpm.PlanId);
            builder.Entity<ReportPlanMap>().ToTable("ReportPlanMapping");
            builder.Entity<Plan>().HasOne(p => p.State).WithMany(s => s.Plans).HasForeignKey(p => p.StateId);
            builder.Entity<Plan>().ToTable("Plans");
            builder.Entity<State>().ToTable("States");
            
            builder.Entity<BusinessContact>().HasMany(bc => bc.Reports).WithOne(r => r.BusinessContact).HasForeignKey(r => r.BusinessContactId);
            builder.Entity<BusinessContact>().ToTable("BusinessContacts");
            
            builder.Entity<UserLog>().HasOne(ul => ul.User).WithMany(usr => usr.UserLogs).HasForeignKey(ul => ul.UserId);
            builder.Entity<UserLog>().ToTable("UserLogs");
            builder.Entity<ReportDeadline>().HasOne(rd => rd.Report).WithMany(rpt => rpt.Deadlines).HasForeignKey(rd => rd.ReportId);
            builder.Entity<ReportDeadline>().ToTable("ReportDeadlines");
        }

        
    }
}
