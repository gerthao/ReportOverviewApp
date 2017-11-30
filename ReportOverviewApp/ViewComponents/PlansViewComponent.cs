using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.ViewComponents
{
    public class PlansViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public PlansViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? reportId, string planName, IEnumerable<int> plans, bool changed=false, bool remove=false)
        {
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Options = new DropdownOptions()
                {
                    BusinessContacts = await _context.BusinessContacts.OrderBy(bc => bc.Name).ToListAsync(),
                    Frequencies = await _context.Reports.Select(r => r.Frequency).Distinct().OrderBy(f => f).ToListAsync(),
                    Plans = await _context.Plans.Include(p => p.State).OrderBy(p => p.Name).ToListAsync(),
                    SourceDepartments = await _context.Reports.Select(r => r.SourceDepartment).Distinct().OrderBy(sd => sd).ToListAsync(),
                    States = await _context.States.OrderBy(s => s.PostalAbbreviation).ToListAsync(),
                    BusinessOwners = null
                }
            };
            if (reportId == null)
            {
                reportViewModel.Report = new Report();
                return View(reportViewModel);
            }
            Report report = await _context.Reports
                                               //.Include(r => r.Deadlines)
                                               //.Include(r => r.BusinessContact)
                                               .Include(r => r.ReportPlanMapping)
                                                    .ThenInclude(rpm => rpm.Plan)
                                                    .ThenInclude(p => p.State)
                                               .Where(r => r.Id == reportId).SingleOrDefaultAsync();
            if (report == null)
            {
                reportViewModel.Report = new Report();
                return View(reportViewModel);
            }
            reportViewModel.Report = report;
            if (changed)
            {
                report.ReportPlanMapping = plans != null && plans.Count() > 0 ? plans.Select(i => new ReportPlanMap() { PlanId = i, ReportId = report.Id }).ToList() : new List<ReportPlanMap>();
                
                //if (!String.IsNullOrEmpty(plans))
                //{
                //    if (plans.Contains(','))
                //    {
                //        list = plans.Split(',');
                //    }
                //    else
                //    {
                //        list = new string[] { plans };
                //    }
                //} else
                //{
                //    list = null;
                //}    
                //if (list != null)
                //{
                //    foreach (string planId in list)
                //    {
                //        Plan plan = await _context.Plans.Include(p => p.State).Where(p => p.Id.ToString() == planId)?.SingleOrDefaultAsync();
                //        if (plan != null)
                //        {
                //            report.ReportPlanMapping.Add(new ReportPlanMap()
                //            {
                //                ReportId = report.Id,
                //                Report = report,
                //                PlanId = plan.Id,
                //                Plan = plan
                //            });
                //        }
                //    }
                //}
            }
            
            
            if (!String.IsNullOrEmpty(planName))
            {
                Plan plan = await _context.Plans.Include(p => p.State).Where(p => p.Name == planName)?.SingleOrDefaultAsync();
                if (plan != null)
                {
                    if (!remove)
                    {
                        if(reportViewModel.Report.ReportPlanMapping == null)
                        {
                            reportViewModel.Report.ReportPlanMapping = new List<ReportPlanMap>();
                        }
                        reportViewModel.Report.ReportPlanMapping.Add(new ReportPlanMap()
                        {
                            ReportId = reportViewModel.Report.Id,
                            Report = report,
                            PlanId = plan.Id,
                            Plan = plan
                        });
                    }
                    else
                    {
                        if(reportViewModel.Report.ReportPlanMapping != null)
                        {
                            for (int i = 0; i < reportViewModel.Report.ReportPlanMapping.Count(); i++)
                            {
                                ReportPlanMap map = reportViewModel.Report.ReportPlanMapping.ElementAt(i);
                                if (map.ReportId == reportViewModel.Report.Id && map.PlanId == plan.Id)
                                {
                                    reportViewModel.Report.ReportPlanMapping.Remove(map);
                                }
                            }
                        }
                        
                    }

                }
            }
            return View(reportViewModel);
        }
    }
}
