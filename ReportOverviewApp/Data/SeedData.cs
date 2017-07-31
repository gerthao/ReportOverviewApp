using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using ReportOverviewApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
                    try
                    {
                        string jsonData = File.ReadAllText(@"C:\Users\gthao\Desktop\crc.json");
                        List<ReportJsonData> products = JsonConvert.DeserializeObject<List<ReportJsonData>>(jsonData);
                        List<Report> reports = new List<Report>();
                        reports.AddRange(from r in products select r.ToReport());
                        context.Reports.AddRange(reports);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                if (context.Reports.Any())
                {
                    context.Reports.ForEachAsync(r => r.NearestDeadline = r.Deadline());
                }
            }
        }
    }
}