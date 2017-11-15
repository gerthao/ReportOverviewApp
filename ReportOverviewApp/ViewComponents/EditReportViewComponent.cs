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
    public class EditReportViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public EditReportViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? reportId)
        {
            if(reportId == null)
            {
                return View(new ReportViewModel());
            }
            Report report = await _context.Reports.Include(r => r.Deadlines)
                                               .Include(r => r.BusinessContact)
                                               .Include(r => r.ReportPlanMapping)
                                                    .ThenInclude(rpm => rpm.Plan)
                                                    .ThenInclude(p => p.State)
                                               .Where(r => r.Id == reportId).SingleOrDefaultAsync();
            if (report == null)
            {
                return View(new ReportViewModel());
            }
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Report = report,
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
            return View(reportViewModel);
        }
    }
}
