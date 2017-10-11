using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class UserLogListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public UserLogListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default", await _context.UserLogs.Include(ul => ul.User).OrderByDescending(ul => ul.TimeStamp).ToListAsync());
        }
    }
}
