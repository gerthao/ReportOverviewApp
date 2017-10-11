using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models.ReportViewModels;
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
        public async Task<IViewComponentResult> InvokeAsync(DateTime? date)
        {
            var viewModel = new ReportDeadlineViewModel() {
                ReportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report).ToListAsync()
            };
            if (date != null && date.HasValue)
            {
                viewModel.Deadline = date;
                return View(viewModel);
            }
            return View(viewModel);
        }
    }
}
