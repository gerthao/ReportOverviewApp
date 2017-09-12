using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOverviewApp.Models
{

    public class Report
    {
        
        [Column(Order = 0)]
        public int ID { get; set; }
        [StringLength(1000), Column(Order = 1)]
        public string Name { get; set; }

        [Display(Name = "Finished")]
        public bool Done { get; set; }
        [Display(Name = "Notified")]
        public bool ClientNotified { get; set; }
        [Display(Name = "Sent")]
        public bool Sent { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), Display(Name = "Finished On")]
        public DateTime? DateDone { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), Display(Name= "Notified On")]
        public DateTime? DateClientNotified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), Display(Name = "Sent On")]
        public DateTime? DateSent { get; set; }
        
        [StringLength(255), Display(Name = "Business Contact")]
        public string BusinessContact { get; set; }
        [StringLength(255), Display(Name = "Business Owner")]
        public string BusinessOwner { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? DueDate1 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? DueDate2 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? DueDate3 { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "No date given")]
        public DateTime? DueDate4 { get; set; }
        [StringLength(50)]
        public string Frequency { get; set;}
        [StringLength(10)]
        public string DayDue { get; set; }
        [StringLength(1000), Display(Name = "Delivery Function")]
        public string DeliveryFunction { get; set; }
        [Display(Name = "Work Instructions")]
        public string WorkInstructions { get; set; }
        public string Notes{ get; set; }
        [Display(Name = "Days After Quarter")]
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
        public bool OtherDepartment { get; set; }
        [StringLength(100), Display(Name = "Source Department")]
        public string SourceDepartment { get; set; }
        [Display(Name = "Quality Indicator")]
        public bool QualityIndicator { get; set; }
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

        /// <summary>
        ///  Gets the closest deadline for a report from the current date.
        /// </summary>
        /// <returns>
        ///  Returns a Nullable DateTime object representating the current deadline of a report.
        /// </returns>
        public DateTime? Deadline(DateTime compareDate)
        {
            if (compareDate == null) compareDate = DateTime.Today;
            switch (Frequency)
            {
                case "Quarterly":
                    return GetDeadlineQuarterly(compareDate);
                case "Monthly":
                    return GetDeadlineMonthly(compareDate);
                case "Weekly":
                    return GetDeadlineWeekly(compareDate);
                case "Biweekly":
                    return GetDeadlineBiweekly(compareDate);
                case "Annual":
                    return GetDeadlineAnnual(compareDate);
                case "Semiannual":
                    return GetDeadlineSemiannual(compareDate);
                default:
                    return null;
            }
        }
        public DateTime? CurrentDeadline()
        {
            return Deadline(DateTime.Today);
        }

        private DateTime? GetDeadlineQuarterly(DateTime compareDate)
        {
            List<DateTime?> deadlines = GetAllDueDates();
            if (deadlines.Count() == 0) return null;
            for (int i = 0; i < deadlines.Count() - 1; i++)
            {
                DateTime? d1 = deadlines[i];
                DateTime? d2 = deadlines[i + 1];
                if (d1 != null && (d1.Value.Year > compareDate.Year + 1 || d1.Value.Year < compareDate.Year)){
                    d1 = new DateTime(year: compareDate.Year, month: d1.Value.Month, day: d1.Value.Day);
                }
                if (d2 != null){
                    d2 = new DateTime(year: d2.Value.Month < d1.Value.Month ? compareDate.Year + 1 : compareDate.Year, month: d2.Value.Month, day: d2.Value.Day);
                }
                deadlines[i] = d1;
                deadlines[i + 1] = d2;
            }
            deadlines = deadlines.Where(date => date >= compareDate).OrderBy(date => date).ToList();
            if (deadlines.Count() == 0) return null;
            return deadlines[0];
        }
        private DateTime? GetDeadlineMonthly(DateTime compareDate)
        {
            try
            {
                DateTime? deadline = new DateTime(year: compareDate.Year, month: compareDate.Month, day: Int32.Parse(DayDue));
                if (deadline < compareDate) deadline = deadline.Value.AddMonths(1);
                return deadline;
            } catch(Exception e)
            {
                Console.WriteLine("Error with GetDeadlineMonthy(DateTime compareDate): \"e.Message\"");
                return null;
            }
        }
        private DateTime? GetDeadlineWeekly(DateTime compareDate)
        {
            DayOfWeek weeklyDueDay = DayOfWeek.Sunday;
            DateTime deadline = compareDate;
            const int daily = 1;
            switch (DayDue)
            {
                case "SUN":
                    weeklyDueDay = DayOfWeek.Sunday;
                    break;
                case "MON":
                    weeklyDueDay = DayOfWeek.Monday;
                    break;
                case "TUE":
                    weeklyDueDay = DayOfWeek.Tuesday;
                    break;
                case "WED":
                    weeklyDueDay = DayOfWeek.Wednesday;
                    break;
                case "THU":
                    weeklyDueDay = DayOfWeek.Thursday;
                    break;
                case "FRI":
                    weeklyDueDay = DayOfWeek.Friday;
                    break;
                case "SAT":
                    weeklyDueDay = DayOfWeek.Saturday;
                    break;
                default:
                    return null;
            }
            while (deadline.DayOfWeek != weeklyDueDay)
            {
                deadline = deadline.AddDays(daily);
            }
            return deadline;
        }
        private DateTime? GetDeadlineBiweekly(DateTime compareDate)
        {
            int biweeklyDay;
            const int biweek = 14, firstBiweek = 15, secondBiweek = 29;
            if (!Int32.TryParse(DayDue, out biweeklyDay))
            {
                return null;
            }
            DateTime deadline = new DateTime(year: compareDate.Year, month: compareDate.Month, day: biweeklyDay);
            if(deadline < compareDate)
            {
                switch (deadline.Day)
                {
                    case firstBiweek:
                        deadline = deadline.AddDays(biweek);
                        break;
                    case secondBiweek:
                        deadline = new DateTime(year: deadline.Year, month: deadline.Month+1, day: biweeklyDay);
                        break;
                    default:
                        deadline = deadline.AddDays(biweek);
                        break;
                }
            }
            return deadline;
        }
        private List<DateTime?> GetAllDueDates()
        {
            List<DateTime?> dates = new List<DateTime?> { DueDate1, DueDate2, DueDate3, DueDate4 };
            dates.RemoveAll(dueDate => dueDate == null);
            return dates;
        }
        private DateTime? GetDeadlineAnnual(DateTime compareDate)
        {
            List<DateTime?> dates = GetAllDueDates();
            if (dates.Count == 0) return null;
            dates.ForEach(date => date = new DateTime(
                                                year: DeadlineHasPassed(date.Value)?compareDate.Year+1:compareDate.Year, 
                                                month: date.Value.Month, 
                                                day: date.Value.Day));
            return dates.ElementAt(0);
        }
        private bool DeadlineHasPassed(DateTime deadline)
        {
            deadline = new DateTime(year: DateTime.Today.Year, month: deadline.Month, day: deadline.Day);
            return deadline < DateTime.Today;
        }
        private DateTime? GetDeadlineSemiannual(DateTime compareDate)
        {
            List<DateTime?> dates = GetAllDueDates();
            if (dates.Count == 0) return null;
            dates.ForEach(date => date = new DateTime(
                                                year: DeadlineHasPassed(date.Value)?compareDate.Year+1:compareDate.Year,
                                                month: date.Value.Month,
                                                day: date.Value.Day));
            return dates.Where(date => date.Value >= DateTime.Today)
                        .OrderBy(date => date)
                        .First();
        }
        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        public Report Copy()
        {
            Report report = new Report();
            report.ID = ID;
            report.Name = Name;
            report.BusinessContact = BusinessContact;
            report.BusinessOwner = BusinessOwner;
            report.DueDate1 = DueDate1;
            report.DueDate2 = DueDate2;
            report.DueDate3 = DueDate3;
            report.DueDate4 = DueDate4;
            report.DateAdded = DateAdded;
            report.EffectiveDate = EffectiveDate;
            report.TerminationDate = TerminationDate;
            report.SystemRefreshDate = SystemRefreshDate;
            report.Frequency = Frequency;
            report.DayDue = DayDue;
            report.DeliveryFunction = DeliveryFunction;
            report.DeliveryMethod = DeliveryMethod;
            report.DeliverTo = DeliverTo;
            report.WorkInstructions = WorkInstructions;
            report.DaysAfterQuarter = DaysAfterQuarter;
            report.FolderLocation = FolderLocation;
            report.ReportType = ReportType;
            report.RunWith = RunWith;
            report.Notes = Notes;
            report.DaysAfterQuarter = DaysAfterQuarter;
            report.GroupName = GroupName;
            report.State = State;
            report.ReportPath = ReportPath;
            report.OtherDepartment = OtherDepartment;
            report.SourceDepartment = SourceDepartment;
            report.QualityIndicator = QualityIndicator;
            report.ERSReportLocation = ERSReportLocation;
            report.ERRStatus = ERRStatus;
            report.LegacyReportID = LegacyReportID;
            report.LegacyReportIDR2 = LegacyReportIDR2;
            report.ERSReportName = ERSReportName;
            report.OtherReportLocation = OtherReportLocation;
            report.OtherReportName = OtherReportName;
            return report;
        }
    }
}
