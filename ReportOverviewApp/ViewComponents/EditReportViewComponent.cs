using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
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
                return View(new Report());
            }
            Report report = await _context.Reports.Include(r => r.Deadlines)
                                               .Include(r => r.BusinessContact)
                                               .Include(r => r.ReportPlanMapping)
                                                    .ThenInclude(rpm => rpm.Plan)
                                                    .ThenInclude(p => p.State)
                                               .Where(r => r.Id == reportId).SingleOrDefaultAsync();
            if (report == null)
            {
                return View(new Report());
            }
            return View(report);
        }
    }
}
