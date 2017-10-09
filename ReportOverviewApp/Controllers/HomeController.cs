using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReportOverviewApp.Data;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models.HomeViewModels;
using NToastNotify;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IToastNotification _toastNotification;

        public HomeController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IActionResult TimeViewComponent()
        {
            return ViewComponent("TimeViewComponent");
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                Reports = await _context.Reports.Include(r => r.Deadlines).ToListAsync(),
                ReportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report).ToListAsync(),
                Users = await _context.Users.ToDictionaryAsync(usr => usr.Id, usr => usr.UserName),
                UserLogs = await _context.UserLogs.ToListAsync()
            };
            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
