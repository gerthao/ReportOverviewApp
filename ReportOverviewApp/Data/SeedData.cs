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
using System.Threading.Tasks;

namespace ReportOverviewApp.Data{
    public static class SeedData{
        private static async void RemoveData(ApplicationDbContext context)
        {
            context.BusinessContacts.RemoveRange(context.BusinessContacts);
            context.ReportDeadlines.RemoveRange(context.ReportDeadlines);
            context.States.RemoveRange(context.States);
            context.Plans.RemoveRange(context.Plans);
            context.Reports.RemoveRange(context.Reports);
            context.ReportPlanMapping.RemoveRange(context.ReportPlanMapping);
            await context.SaveChangesAsync();
        }
        public static void Initialize(IServiceProvider serviceProvider){
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>())){
                if (context.Reports == null || context.Reports.Count() <= 0)
                {
                    //try
                    //{
                    string jsonStateData;

                    try { jsonStateData = File.ReadAllText(@"C:\Users\gthao\Desktop\states.json"); }
                    catch { jsonStateData = File.ReadAllText(@"C:\Users\Ger\Desktop\states.json"); }
                    List<State> states = JsonConvert.DeserializeObject<List<State>>(jsonStateData).ToList();



                    context.States.AddRange(states);
                    context.SaveChanges();

                    string jsonReportData;
                    try { jsonReportData = File.ReadAllText(@"C:\Users\gthao\Desktop\crc4.json"); }
                    catch { jsonReportData = File.ReadAllText(@"C:\Users\Ger\Desktop\crc4.json"); }
                    var results = JsonConvert.DeserializeObject<List<ReportJsonData>>(jsonReportData);
                    List<(Report, Plan)> reportPairs = results.Select(jsonReport => jsonReport.ToReport()).ToList();
                    List<Report> reports = reportPairs.Select(pair => pair.Item1).ToList();
                    List<Plan> plans = reportPairs.Select(pair => pair.Item2).Where(plan => plan != null).GroupBy(plan => plan.Name).Select(group => group.First()).ToList();
                    List<Plan> plansToDb = new List<Plan>();
                    for (int i = 0; i < plans.Count(); i++)
                    {
                        if (plans[i] != null && !String.IsNullOrEmpty(plans[i].Name) && !plansToDb.Where(p => p.Name == plans[i].Name).Any())
                        {
                            if (plans[i] != null && plans[i].State != null && !String.IsNullOrEmpty(plans[i].State.PostalAbbreviation) && context.States.Where(s => s.PostalAbbreviation == plans[i].State.PostalAbbreviation).Any())
                            {
                                plans[i].State = context.States.Where(state => state.PostalAbbreviation == plans[i].State.PostalAbbreviation).First();
                                plans[i].StateId = plans[i].State.Id;
                            }
                            plansToDb.Add(plans[i]);
                                
                        }
                        if(plans[i] == null)
                        {
                            plansToDb.Add(plans[i]);
                        }
                    }
                    context.Plans.AddRange(plansToDb);
                    context.SaveChanges();
                    List<BusinessContact> businessContacts = reports.Select(report => report.BusinessContact).GroupBy(bc => bc.Name).Select(g => g.First()).OrderBy(bc => bc.Name).ToList();
                    //List<BusinessContact> businessContactsToDb = new List<BusinessContact>();
                    //foreach (BusinessContact bc in businessContacts)
                    //{
                    //    if (!context.BusinessContacts.Where(c => c.Name == bc.Name).Any())
                    //    {
                    //        businessContactsToDb.Add(bc);
                                
                    //    }
                    //}
                    context.BusinessContacts.AddRange(businessContacts);
                    context.SaveChanges();

                    for (int i = 0; i < reports.Count; i++)
                    {
                        BusinessContact matchedBusinessContact = context.BusinessContacts.Where(bc => bc.Name == reports[i].BusinessContact.Name).First();
                        reports[i].BusinessContact = matchedBusinessContact;
                        reports[i].BusinessContactId = matchedBusinessContact.Id;
                    }
                    context.Reports.AddRange(reports);
                    context.SaveChanges();

                    List<ReportPlanMap> rpmToDb = new List<ReportPlanMap>();
                    for (int i = 0; i < reportPairs.Count(); i++)
                    {
                        if(reportPairs[i].Item1 != null && reportPairs[i].Item2 != null && !String.IsNullOrEmpty(reportPairs[i].Item2.Name))
                        {
                            (Report, Plan) pair = (context.Reports.Where(r => r.Name == reportPairs[i].Item1.Name).First(), context.Plans.Where(r => r.Name == reportPairs[i].Item2.Name).First());
                            ReportPlanMap map = new ReportPlanMap()
                            {
                                ReportId = pair.Item1.Id,
                                PlanId = pair.Item2.Id
                            };
                            rpmToDb.Add(map);
                        }
                            
                    }
                    context.ReportPlanMapping.AddRange(rpmToDb);
                    context.SaveChanges();

                    List<Report> reportList = context.Reports.ToList();
                    for (int i = 0; i < reportList.Count(); i++)
                    {
                        reportList[i].Name = reportList[i].Name.Split(':')[1];
                    }
                    context.UpdateRange(reportList);
                    context.SaveChanges();

                    //List<ReportDeadline> deadlines = context.Reports.Where(r => r.CurrentDeadline() != null && r.CurrentDeadline().HasValue).Select(r => new ReportDeadline() { ReportId = r.Id, Deadline = r.CurrentDeadline().Value }).ToList();
                    //context.ReportDeadlines.AddRange(deadlines);
                    //context.SaveChanges();

                    
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}

                }
            }

        }
    }
}