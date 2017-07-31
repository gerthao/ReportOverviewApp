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
        [DataType(DataType.Date)]
        public DateTime? Deadline()
        {
            DateTime? dateTime = null;
            switch (Frequency)
            {
                case "Quarterly":
                    return GetDeadlineQuarterly();
                case "Monthly":
                    return GetDeadlineMonthly();
                case "Weekly":
                    return GetDeadlineWeekly();
                case "Biweekly":
                    return GetDeadlineBiweekly();
                case "Annual":
                    return GetDeadlineAnnual();
                case "Semiannual":
                    return GetDeadlineSemiannual();
                default:
                    break;
            }
            return dateTime;
        }
        private DateTime? GetDeadlineQuarterly()
        {
            List<DateTime?> deadlines = GetAllDueDates();
            if (deadlines.Count() == 0) return null;
            for (int i = 0; i < deadlines.Count() - 1; i++)
            {
                DateTime? d1 = deadlines[i];
                DateTime? d2 = deadlines[i + 1];
                if (d1 != null && (d1.Value.Year > DateTime.Now.Year + 1 || d1.Value.Year < DateTime.Now.Year)){
                    d1 = new DateTime(year: DateTime.Now.Year, month: d1.Value.Month, day: d1.Value.Day);
                }
                if (d2 != null){
                    d2 = new DateTime(year: d2.Value.Month < d1.Value.Month ? DateTime.Now.Year + 1 : DateTime.Now.Year, month: d2.Value.Month, day: d2.Value.Day);
                }
                deadlines[i] = d1;
                deadlines[i + 1] = d2;
            }
            deadlines = deadlines.Where(date => date >= DateTime.Now).OrderBy(date => date).ToList();
            if (deadlines.Count() == 0) return null;
            return deadlines[0];
        }
        private DateTime? GetDeadlineMonthly()
        {
            DateTime? deadline = new DateTime(year: DateTime.Now.Year, month: DateTime.Now.Month, day: Int32.Parse(DayDue));
            if (deadline < DateTime.Now) deadline = deadline.Value.AddMonths(1);
            return deadline;
        }
        private DateTime? GetDeadlineWeekly()
        {
            DayOfWeek weeklyDueDay = DayOfWeek.Sunday;
            DateTime deadline = DateTime.Today;
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
        private DateTime? GetDeadlineBiweekly()
        {
            int biweeklyDay;
            const int biweek = 14, firstBiweek = 15, secondBiweek = 29;
            if (!Int32.TryParse(DayDue, out biweeklyDay))
            {
                return null;
            }
            DateTime deadline = new DateTime(year: DateTime.Today.Year, month: DateTime.Today.Month, day: biweeklyDay);
            if(deadline < DateTime.Today)
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
        private DateTime? GetDeadlineAnnual()
        {
            List<DateTime?> dates = GetAllDueDates();
            if (dates.Count == 0) return null;
            dates.ForEach(date => date = new DateTime(
                                                year: DeadlineHasPassed(date.Value)?DateTime.Now.Year+1:DateTime.Now.Year, 
                                                month: date.Value.Month, 
                                                day: date.Value.Day));
            return dates.ElementAt(0);
        }
        private bool DeadlineHasPassed(DateTime deadline)
        {
            deadline = new DateTime(year: DateTime.Today.Year, month: deadline.Month, day: deadline.Day);
            return deadline < DateTime.Today;
        }
        private DateTime? GetDeadlineSemiannual()
        {
            List<DateTime?> dates = GetAllDueDates();
            if (dates.Count == 0) return null;
            dates.ForEach(date => date = new DateTime(
                                                year: DeadlineHasPassed(date.Value)?DateTime.Now.Year+1:DateTime.Now.Year,
                                                month: date.Value.Month,
                                                day: date.Value.Day));
            return dates.Where(date => date.Value >= DateTime.Today)
                        .OrderBy(date => date)
                        .First();
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
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
