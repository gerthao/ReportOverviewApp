using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.ViewComponents
{
    [ViewComponent]
    public class TimeViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public TimeViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default", await GetCurrentDateTimeAsync());
        }
        private async Task<string> GetCurrentTimeAsync()
        {
            var currentTime = await Task.FromResult<string>(DateTime.Now.ToShortTimeString());
            return currentTime;
        }
        private async Task<DateTime> GetCurrentDateTimeAsync()
        {
            var currentDateTime = await Task.FromResult<DateTime>(DateTime.Now);
            return currentDateTime;
        }
    }
}
