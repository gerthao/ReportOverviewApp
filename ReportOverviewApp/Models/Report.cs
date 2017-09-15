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
        ///  Gets the closest deadline for a report based on the parameter and frequency type of the Report.
        /// </summary>
        /// <param name="compareDate">
        ///  Parameter compareDate is used to compared which day to look for deadlines.
        ///  If null then compareDate becomes today's date.
        /// </param>
        /// <returns>
        ///  Returns a Nullable DateTime object representating the current deadline of a report.
        /// </returns>
        public DateTime? Deadline(DateTime compareDate)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine($"Something went wrong: { e.ToString()}");
            }
            return null;
        }
        /// <summary>
        /// Gets the closest deadline for a report from today.
        /// </summary>
        /// <returns></returns>
        public DateTime? CurrentDeadline()
        {
            return Deadline(DateTime.Today);
        }

        private DateTime? GetDeadlineQuarterly(DateTime compareDate)
        {
            List<DateTime?> deadlines = GetAllDueDates(compareDate);
            const int quarterlyLimit = 4;
            if (deadlines.Count() != quarterlyLimit) return null;
            var current = deadlines.Where(e => e.Value >= compareDate).Take(1).Single();
            if (deadlines.Count() == 0) return null;
            return current;
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
                Console.WriteLine($"Error with GetDeadlineMonthy(DateTime compareDate): \"{e.Message}\"");
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
                case "Sunday":
                    weeklyDueDay = DayOfWeek.Sunday;
                    break;
                case "MON":
                case "Monday":
                    weeklyDueDay = DayOfWeek.Monday;
                    break;
                case "TUE":
                case "Tuesday":
                    weeklyDueDay = DayOfWeek.Tuesday;
                    break;
                case "WED":
                case "Wednesday":
                    weeklyDueDay = DayOfWeek.Wednesday;
                    break;
                case "THU":
                case "Thursday":
                    weeklyDueDay = DayOfWeek.Thursday;
                    break;
                case "FRI":
                case "Friday":
                    weeklyDueDay = DayOfWeek.Friday;
                    break;
                case "SAT":
                case "Saturday":
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
        private List<DateTime?> GetAllDueDates(DateTime date)
        {
            List<DateTime?> dates = GetAllDueDates();
            for(int i = 0; i < dates.Count(); i++)
            {
                dates[i] = new DateTime(year: date.Year, month: dates[i].Value.Month, day: dates[i].Value.Day);
                if(dates[i].Value < date)
                {
                    dates[i] = dates[i].Value.AddYears(1);
                }
            }
            dates.Sort();
            return dates;
        }
        private DateTime? GetDeadlineAnnual(DateTime compareDate)
        {
            const int annualLimit = 1;
            List<DateTime?> dates = GetAllDueDates(compareDate);
            if (dates.Count != annualLimit) return null;
            return dates.ElementAt(0);
        }
        private bool DeadlineHasPassed(DateTime deadline)
        {
            deadline = new DateTime(year: DateTime.Today.Year, month: deadline.Month, day: deadline.Day);
            return deadline < DateTime.Today;
        }
        private DateTime? GetDeadlineSemiannual(DateTime compareDate)
        {
            const int semiannualLimit = 2;
            List<DateTime?> dates = GetAllDueDates(compareDate);
            if (dates.Count != semiannualLimit) return null;
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
