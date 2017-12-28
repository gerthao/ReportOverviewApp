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
        public async Task<IViewComponentResult> InvokeAsync(int? deadlineId)
        {
            if (deadlineId == null) return View(new ReportDeadline());
            if(await _context.ReportDeadlines.FindAsync(deadlineId) != null)
            {
                return View(await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Id == deadlineId).SingleOrDefaultAsync());
            }
            return View(new ReportDeadline());
        }
    }
}
