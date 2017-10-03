using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public abstract class AbstractReport
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [StringLength(1000), Column(Order = 1)]
        public string Name { get; set; }

        public virtual ICollection<ReportDeadline> Deadlines { get; set; }

        [StringLength(255), Display(Name = "Business Contact")]
        public string BusinessContact { get; set; }
        [StringLength(255), Display(Name = "Business Owner")]
        public string BusinessOwner { get; set; }
        [StringLength(50)]
        public string Frequency { get; set; }
        [StringLength(10)]
        public string DayDue { get; set; }
        [StringLength(1000), Display(Name = "Delivery Function")]
        public string DeliveryFunction { get; set; }
        [Display(Name = "Work Instructions")]
        public string WorkInstructions { get; set; }
        public string Notes { get; set; }
        [Display(Name = "Days After Quarter (If Applicable)")]
        public int? DaysAfterQuarter { get; set; }
        [StringLength(2000), Display(Name = "Folder Location")]
        public string FolderLocation { get; set; }
        [StringLength(50), Display(Name = "Report Type")]
        public string ReportType { get; set; }
        [StringLength(100), Display(Name = "Run With")]
        public string RunWith { get; set; }
        [StringLength(260), Display(Name = "Delivery Method")]
        public string DeliveryMethod { get; set; }
        [Display(Name = "Deliver To")]
        public string DeliverTo { get; set; }
        [DataType(DataType.Date), Display(Name = "Effective Date")]
        public DateTime? EffectiveDate { get; set; }
        [DataType(DataType.Date), Display(Name = "Termination Date")]
        public DateTime? TerminationDate { get; set; }
        [StringLength(255), Display(Name = "Plan")]
        public string GroupName { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(2000), Display(Name = "Report Path")]
        public string ReportPath { get; set; }
        [Display(Name = "Other Department")]
        public bool IsFromOtherDepartment { get; set; }
        [StringLength(100), Display(Name = "Source Department")]
        public string SourceDepartment { get; set; }
        [Display(Name = "Quality Indicator")]
        public bool IsQualityIndicator { get; set; }
        [StringLength(4000), Display(Name = "ERS Report Location")]
        public string ERSReportLocation { get; set; }
        [Display(Name = "ERR Status")]
        public int? ERRStatus { get; set; }
        [Display(Name = "Date Added"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? DateAdded { get; set; }
        [Display(Name = "System Refresh Date"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ConvertEmptyStringToNull = true, ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? SystemRefreshDate { get; set; }
        [Display(Name = "Legacy Report ID")]
        public int? LegacyReportID { get; set; }
        [Display(Name = "Legacy Report ID R2")]
        public int? LegacyReportIDR2 { get; set; }
        [StringLength(1000), Display(Name = "ERS Report Name")]
        public string ERSReportName { get; set; }
        [StringLength(4000), Display(Name = "Other Report Location")]
        public string OtherReportLocation { get; set; }
        [StringLength(1000), Display(Name = "Other Report Name")]
        public string OtherReportName { get; set; }

        public abstract DateTime? Deadline(DateTime compareDate);
        public abstract DateTime? CurrentDeadline();
    }
}
