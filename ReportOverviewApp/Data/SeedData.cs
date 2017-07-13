using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (!context.Reports.Any())
                {
                    //try
                    //{
                        string jsonData = @"{'REPORT_NAME':'GHP Kids Patient Count by Location ','BUSINESS_CONTACT':'Debbie Chapin','BUSINESS_OWNER':'Client Engagement','DAY_DUE':'30','FREQUENCY':'Quarterly','DELIVERY_FUNCTION':null,'WORK_INSTRUCTIONS':'LINK:  enterprisereporting/Reports/Pages/Report.aspx?ItemPath=%2fDentaQuest%2fClient+Reporting%2fPatient+Count+by+Location','NOTES':'No Notes Available','DAYS_AFTER_QUARTER':60,'FOLDER_LOCATION':'DIRECTORY:  S:\\DoralReports\\Geisinger Health Plan CHIP','REPORT_TYPE':'NULL','RUN_WITH':null,'DELIVERY_METHOD':null,'DELIVER_TO':null,'GROUP_NAME':'Geisinger Health Plan CHIP','WW_GROUP_ID':7002601001,'STATE':'PA','REPORT_PATH':null,'Other Department':'N','Source Department':'BI Reporting','Quality Indicator':'N','ERS_REPORT_LOCATION':'LINK:  enterprisereporting/Reports/Pages/Report.aspx?ItemPath=%2fDentaQuest%2fClient+Reporting%2fPatient+Count+by+Location','ERR_STATUS':1,'LEGACY_REPORT_ID':null,'LEGACY_REPORT_ID_R2':null,'ERS_REPORT_NAME':'Patient Count by Location','OTHER_REPORT_LOCATION':null,'OTHER_REPORT_NAME':null}";
                        Report product = JsonConvert.DeserializeObject<Report>(jsonData);
                        context.Reports.Add(product);
                        context.SaveChanges();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}
                }
            }
        }
    }
}