using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReportOverviewApp.Data;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models.HomeViewModels;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //IDictionary<string, object> blob = new Dictionary<string, object>();
            //var reports = from r in _context.Reports select r;
            //var users = from u in _context.Users select u;
            //blob.Add("reports", reports);
            //blob.Add("users", users);
            //return View(blob);
            var viewModel = new HomeViewModel()
            {
                Reports = from r in _context.Reports select r
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
