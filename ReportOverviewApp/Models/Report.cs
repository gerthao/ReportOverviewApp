using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOverviewApp.Models
{

    public class Report
    {
        
        [Column(Order = 0)]
        public int ID { get; set; }
        [StringLength(1000), Column(Order = 1)]
        public string Name { get; set; }

        public bool Done { get; set; }
        public bool ClientNotified { get; set; }
        public bool Sent { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateDue { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateDone { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateClientNotified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateSent { get; set; }
        
        [StringLength(255)]
        public string BusinessContact { get; set; }
        [StringLength(255)]
        public string BusinessOwner { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate1 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate2 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate3 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DueDate4 { get; set; }
        [StringLength(50)]
        public string Frequency { get; set;}
        [StringLength(10)]
        public string DayDue { get; set; }
        [StringLength(1000)]
        public string DeliveryFunction { get; set; }
        public string WorkInstructions { get; set; }
        public string Notes{ get; set; }
        [StringLength(20)]
        public int? DaysAfterQuarter { get; set; }
        [StringLength(2000)]
        public string FolderLocation { get; set; }
        [StringLength(50)]
        public string ReportType { get; set; }
        [StringLength(100)]
        public string RunWith { get; set; }
        [StringLength(260)]
        public string DeliveryMethod { get; set; }
        public string DeliveryTo { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EffectiveDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }
        [StringLength(255)]
        public string GroupName { get; set; }
        [StringLength(10)]
        public string State { get; set; }
        [StringLength(2000)]
        public string ReportPath { get; set; }
        public bool OtherDepartment { get; set; }
        [StringLength(100)]
        public string SourceDepartment { get; set; }
        public bool QualityIndicator { get; set; }
        [StringLength(4000)]
        public string ERSReportLocation { get; set; }
        public int? ERRStatus { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateAdded { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SystemRefreshDate { get; set; }
        public int? LegacyReportID { get; set; }
        public int? LegacyReportIDR2 { get; set; }
        [StringLength(1000)]
        public string ERSReportName { get; set; }
        [StringLength(4000)]
        public string OtherReportLocation { get; set; }
        [StringLength(1000)]
        public string OtherReportName { get; set; }

        public DateTime? Deadline()
        {
            DateTime? dateTime = null;
            switch (Frequency)
            {
                case "Quarterly":
                    DateTime[] deadlines = new DateTime[] { DueDate1.Value, DueDate2.Value, DueDate3.Value, DueDate4.Value };
                    for(int i = 0; i < deadlines.Count()-1; i++)
                    {
                        DateTime d1 = deadlines[i];
                        DateTime d2 = deadlines[i + 1];
                        if(d1 >= DateTime.Now && d1 <= d2)
                        {
                            dateTime = d1;
                        } if(d2 >= DateTime.Now && d2 <= d1)
                        {
                            dateTime = d2;
                        }
                    }
                    break;
                case "Monthly":
                    dateTime = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: Int32.Parse(DayDue));
                    break;
                case "Weekly":
                    break;
                case "Biweekly":
                    break;
                case "Annual":
                    break;
                case "Semiannual":
                    break;
                default:
                    break;
            }
            return dateTime;
        }
        //public bool IsPastDue() => DateDue != null && DateDue > DateTime.Now;
        //public bool IsPastDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate < DateDue;
        //public bool IsDue() => DateDue != null && DateDue <= DateTime.Now;
        //public bool IsDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate >= DateDue;
        //public bool IsDone() => DateDone != null;
        //public bool HasBeenSent() => DateSent != null;
        //public bool HasBeenDone() => DateDone != null;
        //public bool HasBeenNotified() => DateClientNotified != null;
    }
}
