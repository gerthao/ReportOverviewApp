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
    public class ReportListViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;
        public ReportListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            

            return View(await _context.Reports.Include(r => r.Deadlines).ToListAsync());
        }
    }
}
