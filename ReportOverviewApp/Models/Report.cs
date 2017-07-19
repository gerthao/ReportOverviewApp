using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{

    public class Report
    {
        

        public int ID { get; set; }
        public string Name { get; set; }

        //[JsonProperty("FREQUENCY")]
        //public FrequencyType Frequency { get; set; }

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
        
        public string BusinessContact { get; set; }
        public string BusinessOwner { get; set; }     
        public DateTime? DueDate1 { get; set; }       
        public DateTime? DueDate2 { get; set; }
        public DateTime? DueDate3 { get; set; }
        public DateTime? DueDate4 { get; set; }
        public ReportEnum.FrequencyType Frequency { get; set;}
        public string DayDue { get; set; }
        public string DeliveryFunction { get; set; }
        public string WorkInstructions { get; set; }
        public string Notes{ get; set; }
        public int? DaysAfterQuarter { get; set; }
        public string FolderLocation { get; set; }
        public string ReportType { get; set; }
        public string RunWith { get; set; }
        public string DeliveryMethod { get; set; }
        public string DeliveryTo { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string GroupName { get; set; }
        public string State { get; set; }
        public string ReportPath { get; set; }
        public bool OtherDepartment { get; set; }
        public string SourceDepartment { get; set; }
        public bool QualityIndicator { get; set; }
        public string ERSReportLocation { get; set; }
        public int? ERRStatus { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? SystemRefreshDate { get; set; }
        public int? LegacyReportID { get; set; }
        public int? LegacyReportIDR2 { get; set; }
        public string ERSReportName { get; set; }
        public string OtherReportLocation { get; set; }
        public string OtherReportName { get; set; }

        public DateTime? Deadline()
        {
            DateTime? dateTime = null;
            switch (Frequency)
            {
                case ReportEnum.FrequencyType.Quarterly:
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
                case ReportEnum.FrequencyType.Monthly:
                    dateTime = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: Int32.Parse(DayDue));
                    break;
                case ReportEnum.FrequencyType.Weekly:
                    break;
                default:
                    break;
            }
            return dateTime;
        }
        public bool IsPastDue() => DateDue != null && DateDue > DateTime.Now;
        public bool IsPastDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate < DateDue;
        public bool IsDue() => DateDue != null && DateDue <= DateTime.Now;
        public bool IsDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate >= DateDue;
        public bool IsDone() => DateDone != null;
        public bool HasBeenSent() => DateSent != null;
        public bool HasBeenDone() => DateDone != null;
        public bool HasBeenNotified() => DateClientNotified != null;
    }
}
