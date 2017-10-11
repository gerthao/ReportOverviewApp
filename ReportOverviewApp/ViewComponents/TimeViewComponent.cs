using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.ViewComponents
{
    public class TimeViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public TimeViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IViewComponentResult Invoke()
        //{
        //    return View("Default", DateTime.Now);
        //}
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default", await GetCurrentDateTimeAsync());
        }
        private async Task<string> GetCurrentTimeAsync()
        {
            DateTime current = DateTime.Now;
            return await Task.FromResult(current.ToShortTimeString());
        }
        private async Task<DateTime> GetCurrentDateTimeAsync()
        {
            var currentDateTime = await Task.FromResult<DateTime>(DateTime.Now);
            return currentDateTime;
        }
    }
}
