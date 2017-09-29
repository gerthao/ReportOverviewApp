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

namespace ReportOverviewApp.Data{
    public static class SeedData{
        public static void Initialize(IServiceProvider serviceProvider){
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())){
                //if (context.ReportDeadlines.Any())
                //{
                //    context.ReportDeadlines.RemoveRange(context.ReportDeadlines);
                //    context.SaveChanges();
                //}
                //if (context.Reports.Any())
                //{
                //    context.Reports.RemoveRange(context.Reports);
                //    context.SaveChanges();
                //}
                
                if (!context.Reports.Any())
                {
                    try
                    {
                        string jsonData;
                        try { jsonData = File.ReadAllText(@"C:\Users\gthao\Desktop\crc.json"); }
                        catch { jsonData = File.ReadAllText(@"C:\Users\Ger\Desktop\crc.json"); }
                        List<Report> reports = JsonConvert.DeserializeObject<List<ReportJsonData>>(jsonData).Select(jsonReport => jsonReport.ToReport()).ToList();


                        context.Reports.AddRange(reports);
                        context.SaveChanges();

                        List<ReportDeadline> deadlines = context.Reports.Where(r => r.CurrentDeadline() != null && r.CurrentDeadline().HasValue).Select(r => new ReportDeadline() { ReportId = r.Id, Deadline = r.CurrentDeadline().Value}).ToList();

                        context.ReportDeadlines.AddRange(deadlines);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}