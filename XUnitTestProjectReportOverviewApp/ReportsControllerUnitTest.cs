using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using ReportOverviewApp.Controllers;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ReportOverviewApp.Models.ReportViewModels;

namespace XUnitTestProjectReportOverviewApp
{
    public enum FrequencyType
    {
        Quarterly, Weekly, Monthly, Biweekly, Semiannual, Annual
    }
    public static class TestReportFactory
    {
        public static int Count { get; private set; } = 1;
        public static Report Create(FrequencyType frequency, string dayDue = null, int daysAfterQuarter = 0)
        {
            Report report = new Report();
            report.Id = Count;
            report.Name = $"Test Report #{Count}";
            report.SourceDepartment = "Test Department";
            //report.GroupName = "Test Plan";
            //report.State = "Test State";
            switch (frequency)
            {
                case FrequencyType.Quarterly:
                    const int baseQuarterlyMonth = 3;
                    report.Frequency = "Quarterly";
                    report.DaysAfterQuarter = daysAfterQuarter;
                    report.DueDate1 = new DateTime(year: 9999, month: baseQuarterlyMonth, day: 1).AddDays(report.DaysAfterQuarter.Value);
                    report.DueDate2 = report.DueDate1.Value.AddMonths(baseQuarterlyMonth);
                    report.DueDate3 = report.DueDate2.Value.AddMonths(baseQuarterlyMonth);
                    report.DueDate4 = report.DueDate3.Value.AddMonths(baseQuarterlyMonth);
                    break;
                case FrequencyType.Weekly:
                    report.Frequency = "Weekly";
                    report.DayDue = dayDue;
                    break;
                case FrequencyType.Monthly:
                    report.Frequency = "Monthly";
                    report.DayDue = "10";
                    break;
            }
            Count++;
            return report;
        }

        
    }
    public class ReportsControllerUnitTest
    {
        private DbSet<Report> GetTestReports()
        {
            var testReports = new List<Report>
            {
                TestReportFactory.Create(FrequencyType.Quarterly, daysAfterQuarter: 15),
                TestReportFactory.Create(FrequencyType.Quarterly, daysAfterQuarter: 45),
                TestReportFactory.Create(FrequencyType.Quarterly, daysAfterQuarter: 30),
                TestReportFactory.Create(FrequencyType.Quarterly, daysAfterQuarter: 60),
                TestReportFactory.Create(FrequencyType.Weekly, dayDue: "Monday"),
                TestReportFactory.Create(FrequencyType.Weekly, dayDue: "Wednesday"),
                TestReportFactory.Create(FrequencyType.Weekly, dayDue: "Friday"),
                TestReportFactory.Create(FrequencyType.Monthly, dayDue: "12")
            };
            DbSet<Report> asdf = testReports.AsQueryable() as DbSet<Report>;
            return asdf;
        }
        //[Fact]
        //public async Task IndexReturnsReportViewModelTestAsync()
        //{
        //    var mockContext = new Mock<ApplicationDbContext>();
        //    mockContext.Setup(context => context.Set<Report>()).Returns(value: GetTestReports());
        //    var controller = new ReportsController(mockContext.Object);
        //    var result = await controller.Index(null, null, 100, 1, null, null, null);
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsAssignableFrom<IEnumerable<ReportViewModel>>(viewResult.ViewData.Model);
        //    Assert.Equal(TestReportFactory.Count, model.Count());
        //}
    }
}
