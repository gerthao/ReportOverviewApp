using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.ViewComponents
{
    public class EditReportDeadlineViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public EditReportDeadlineViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? reportId, int? index)
        {
            if (reportId == null) return View(new ReportDeadline());
            if (index == null) index = 0;
            if(await _context.Reports.FindAsync(reportId) != null)
            {
                var deadlines = await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.ReportId == reportId).OrderByDescending(rd => rd.Deadline).ToListAsync();
                if (index.Value >= 0 && index.Value < deadlines.Count()) return View(deadlines[index.Value]);
            }
            return View(new ReportDeadline());
        }
    }
}
