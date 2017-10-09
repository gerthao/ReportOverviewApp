using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.ViewComponents
{
    public class ReportDeadlineListViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        public ReportDeadlineListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.ReportDeadlines.Include(r => r.Report).ToListAsync());
        }
    }
}
