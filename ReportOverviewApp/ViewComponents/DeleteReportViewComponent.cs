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
    public class DeleteReportViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public DeleteReportViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? reportId)
        {
            if (reportId == null) return View(new Report());
            Report report = await _context.Reports.FindAsync(reportId);
            if (report == null) return View(new Report());
            return View(report);
        }
    }
}
