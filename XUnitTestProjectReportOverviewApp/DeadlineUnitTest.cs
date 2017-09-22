using System;
using Xunit;
using ReportOverviewApp.Models;
using Moq;
using ReportOverviewApp.Data;
using ReportOverviewApp.Helpers;
using ReportOverviewApp.Services;
using ReportOverviewApp.Controllers;

namespace XUnitTestProjectReportOverviewApp
{
    public class DeadlineUnitTest
    {
        [Fact]
        public void ReportDeadlineTestWeekly()
        {
            Report mondayReport = new Report() { DayDue = "Monday", Frequency = "Weekly" };
            Report tuesdayReport = new Report() { DayDue = "Tuesday", Frequency = "Weekly" };
            Report wednesdayReport = new Report() { DayDue = "Wednesday", Frequency = "Weekly" };
            Report monReport = new Report() { DayDue = "MON", Frequency = "Weekly" };
            Report thuReport = new Report() { DayDue = "THU", Frequency = "Weekly" };
            Report fridayReport = new Report() { DayDue = "Friday", Frequency = "Weekly" };
            Report satReport = new Report() { DayDue = "SAT", Frequency = "Weekly" };
            Assert.True(mondayReport.CurrentDeadline().Value.DayOfWeek == DayOfWeek.Monday);
            Assert.True(monReport.CurrentDeadline().Value.DayOfWeek == DayOfWeek.Monday);
            Assert.True(tuesdayReport.CurrentDeadline().Value.DayOfWeek == DayOfWeek.Tuesday);
            Assert.True(tuesdayReport.CurrentDeadline().Value.Month == DateTime.Today.Month);
            Assert.True(fridayReport.CurrentDeadline().Value.DayOfWeek == DayOfWeek.Friday);
            Assert.False(thuReport.CurrentDeadline().Value.DayOfWeek == DayOfWeek.Monday);
            Assert.True(satReport.CurrentDeadline().Value.Year == DateTime.Today.Year);
        }
        [Fact]
        public void ReportDeadlineTestWeeklyBadDeadline()
        {
            Report mondayReport = new Report() { DayDue = "Today", Frequency = "Weekly" };
            Report tueReport = new Report() { DayDue = "TUE", Frequency = "Weekly" };
            Assert.Null(mondayReport.CurrentDeadline());
            Assert.NotNull(tueReport.CurrentDeadline());
        }
        [Fact]
        public void ReportDeadlineTestQuarterly()
        {
            Report report = new Report()
            {
                DueDate1 = new DateTime(year: 9999, month: 3, day: 1),
                DueDate2 = new DateTime(year: 9999, month: 6, day: 1),
                DueDate3 = new DateTime(year: 9999, month: 9, day: 1),
                DueDate4 = new DateTime(year: 9999, month: 12, day: 1),
                Frequency = "Quarterly"
            };
            Assert.True(report.Deadline(new DateTime(year: 2017, month: 3, day: 1)) == new DateTime(year: 2017, month: 3, day: 1));
            Assert.True(report.Deadline(new DateTime(year: 2017, month: 4, day: 1)) == new DateTime(year: 2017, month: 6, day: 1));
            Assert.True(report.Deadline(new DateTime(year: 2017, month: 12, day: 31)) == new DateTime(year: 2018, month: 3, day: 1));
        }
        [Fact]
        public void ReportDeadlineTestQuarterlyBadDeadline()
        {
            Report report1 = new Report()
            {
                DueDate1 = new DateTime(year: 9999, month: 3, day: 1),
                DueDate2 = new DateTime(year: 9999, month: 6, day: 1),
                DueDate3 = null,
                DueDate4 = new DateTime(year: 9999, month: 12, day: 1),
                Frequency = "Quarterly"
            },
            report2 = new Report()
            {
                DueDate1 = new DateTime(year: 9999, month: 4, day: 15),
                DueDate2 = new DateTime(year: 9999, month: 7, day: 15),
                DueDate3 = new DateTime(year: 9999, month: 11, day: 15),
                DueDate4 = new DateTime(year: 9999, month: 1, day: 15),
                Frequency = "Quarterly"
            };
            Assert.Null(report1.CurrentDeadline());
            Assert.NotNull(report2.CurrentDeadline());
        }
        [Fact]
        public void ReportDeadlineTestMonthly()
        {
            Report report = new Report()
            {
                DayDue = "15",
                Frequency = "Monthly"
            };
            Assert.False(report.CurrentDeadline() == null || !report.CurrentDeadline().HasValue);
            Assert.True(report.Frequency == "Monthly");
            Assert.True(report.Deadline(new DateTime(year: 2011, month: 4, day: 15)) == new DateTime(year: 2011, month: 4, day: 15));
            Assert.True(report.Deadline(new DateTime(year: 2011, month: 4, day: 16)) == new DateTime(year: 2011, month: 5, day: 15));
            Assert.True(report.Deadline(new DateTime(year: 2011, month: 12, day: 22)) == new DateTime(year: 2012, month: 1, day: 15));
        }
        [Fact]
        public void ReportDeadlineTestMonthlyBadDeadline()
        {
            Report badReport = new Report()
            {
                DayDue = "bad_string",
                Frequency = "Monthly"
            };
            Assert.Null(badReport.CurrentDeadline());
        }
        [Fact]
        public void ReportDeadlineTestBiweekly()
        {
            DateTime date1 = new DateTime(year: 2013, month: 3, day: 2);
            DateTime date2 = new DateTime(year: 2013, month: 3, day: 14);
            DateTime date3 = new DateTime(year: 2013, month: 3, day: 17);
            DateTime date4 = new DateTime(year: 2013, month: 3, day: 29);
            DateTime date5 = new DateTime(year: 2013, month: 3, day: 30);
            DateTime date6 = new DateTime(year: 2013, month: 3, day: 31);
            DateTime date7 = new DateTime(year: 2013, month: 4, day: 15);
            Report report1 = new Report()
            {
                Frequency = "Biweekly", DayDue = "15"
            };
            Report report2 = new Report()
            {
                Frequency = "Biweekly",
                DayDue = "30"
            };
            Assert.True(report1.Deadline(date1) == new DateTime(year: 2013, month: 3, day: 15));
            Assert.True(report1.Deadline(date2) == new DateTime(year: 2013, month: 3, day: 15));
            Assert.True(report1.Deadline(date3) == new DateTime(year: 2013, month: 3, day: 29));
            Assert.True(report1.Deadline(date4) == new DateTime(year: 2013, month: 3, day: 29));
            Assert.True(report2.Deadline(date1) == new DateTime(year: 2013, month: 3, day: 30));
            Assert.True(report2.Deadline(date5) == new DateTime(year: 2013, month: 3, day: 30));
            Assert.True(report2.Deadline(date6) == new DateTime(year: 2013, month: 4, day: 13));
            Assert.True(report2.Deadline(date7) == new DateTime(year: 2013, month: 4, day: 30));
        }
        [Fact]
        public void ReportDeadlineTestBiweeklyBadDeadline()
        {
            Report report1 = new Report()
            {
                Frequency = "Biweekly",
                DayDue = "Friday"
            };
            Report report2 = new Report()
            {
                Frequency = "Biweekly",
                DueDate1 = new DateTime(year: 2018, month: 5, day: 15),
                DayDue = "15"
            };
            Assert.Null(report1.CurrentDeadline());
            Assert.True(report2.Deadline(report2.DueDate1.Value) == new DateTime(year: 2018, month: 5, day: 15));
        }
        [Fact]
        public void ReportDeadlineTestAnnual()
        {
            Report report1 = new Report()
            {
                Frequency = "Annual",
                DueDate1 = new DateTime(year: 9999, month: 6, day: 15)
            };
            Assert.False(report1.CurrentDeadline() == null || !report1.CurrentDeadline().HasValue);
            Assert.True(report1.Deadline(new DateTime(year: 2000, month: 5, day: 12)) == new DateTime(year: 2000, month: 6, day: 15));
        }
        [Fact]
        public void ReportDeadlineTestAnnualBadDeadline()
        {
            Report report1 = new Report()
            {
                Frequency = "Annual",
                DueDate1 = new DateTime(year: 1234, month: 2, day: 23),
                DueDate2 = new DateTime(year: 1234, month: 2, day: 23)
            };
            Assert.Null(report1.CurrentDeadline());
            Assert.Null(report1.Deadline(new DateTime(year: 1234, month: 2, day: 23)));
        }
        [Fact]
        public void ReportDeadlineTestSemiannual()
        {
            const int semiannualMonthsDuration = 6;
            Report report1 = new Report()
            {
                Frequency = "Semiannual"
            };
            report1.DueDate1 = new DateTime(year: 9999, month: 5, day: 15);
            report1.DueDate2 = report1.DueDate1.Value.AddMonths(semiannualMonthsDuration);
            DateTime dateTime1 = new DateTime(year: 1111, month: 1, day: 1);
            DateTime dateTime2 = new DateTime(year: 1111, month: 6, day: 1);
            DateTime dateTime3 = new DateTime(year: 1111, month: 11, day: 15);
            DateTime dateTime4 = new DateTime(year: 1111, month: 11, day: 16);
            Assert.False(report1.CurrentDeadline() == null || !report1.CurrentDeadline().HasValue);
            Assert.True(report1.Deadline(dateTime1) == new DateTime(year: 1111, month: 5, day: 15));
            Assert.True(report1.Deadline(dateTime2) == new DateTime(year: 1111, month: 11, day: 15));
            Assert.True(report1.Deadline(dateTime3) == new DateTime(year: 1111, month: 11, day: 15));
            Assert.True(report1.Deadline(dateTime4) == new DateTime(year:  1112, month: 5, day: 15));
        }
        [Fact]
        public void ReportDeadlineTestSemiannualBadDeadline()
        {
            const int semiannualMonthsDuration = 6;
            Report report1 = new Report()
            {
                Frequency = "Semiannual"
            };
            report1.DueDate1 = new DateTime(year: 9999, month: 5, day: 15);
            report1.DueDate2 = null;
            Report report2 = new Report()
            {
                DueDate1 = new DateTime(year: 9999, month: 5, day: 15),
                DueDate2 = new DateTime(year: 9999, month: 5, day: 16),
                DueDate3 = new DateTime(year: 9999, month: 5, day: 17),
            };

            DateTime dateTime1 = new DateTime(year: 1111, month: 1, day: 1);
            DateTime dateTime2 = dateTime1.AddMonths(semiannualMonthsDuration);
            Assert.Null(report1.CurrentDeadline());
            Assert.Null(report1.Deadline(dateTime1));
            Assert.Null(report1.Deadline(dateTime2));
            Assert.Null(report2.CurrentDeadline());
            Assert.Null(report2.Deadline(dateTime1));
            Assert.Null(report2.Deadline(dateTime2));
        }
    }
}
