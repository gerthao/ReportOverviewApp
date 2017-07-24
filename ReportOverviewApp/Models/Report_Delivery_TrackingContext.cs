using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReportOverviewApp.Models
{
    public partial class Report_Delivery_TrackingContext : DbContext
    {
        // Unable to generate entity type for table 'dbo.REPORT_DELIVERY'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=wadtisql01d.dqdev.ad;Initial Catalog=Report_Delivery_Tracking;Integrated Security=False;User ID=ETL_Developer;Password=3RR_$dev;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}