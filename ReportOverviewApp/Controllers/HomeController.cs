using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ReportOverviewApp.Data;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models.HomeViewModels;
using ReportOverviewApp.Models.WidgetModels;

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
        private List<Widget> DefaultWidgets()
        {
            WidgetBuilder builder = new WidgetBuilder();
            var wigetContainer = new List<Widget>
            {
                builder
                    .BuildProduct()
                    .BuildID(1)
                    .BuildHeader("Wiget 1")
                    .BuildSubWidgetTopic("All Reports")
                    .BuildSubWidgetAction(WidgetFunctions.ReportCount())
                    .BuildFooter("Footer")
                    .BuildColor("Red")
                    .BuildOption("Edit This Widget")
                    .ReleaseProduct(),

                builder
                    .BuildProduct()
                    .BuildID(2)
                    .BuildHeader("Wiget 2")
                    .BuildSubWidgetTopic("All Reports")
                    .BuildSubWidgetAction(WidgetFunctions.ReportCount(DateTime.Now, DateTime.Now.AddDays(20)))
                    .BuildFooter("Footer")
                    .BuildColor("Black")
                    .BuildOption("Edit This Widget").BuildOption("Delete This Widget")
                    .ReleaseProduct()
            };
            return wigetContainer;
        }
        public ActionResult Index()
        {
            //IDictionary<string, object> blob = new Dictionary<string, object>();
            //var reports = from r in _context.Reports select r;
            //var users = from u in _context.Users select u;
            //blob.Add("reports", reports);
            //blob.Add("users", users);
            //return View(blob);
            var wid = from w in DefaultWidgets() select w;
            var viewModel = new HomeViewModel()
            {
                Reports = from r in _context.Reports select r,
                Widgets = wid,
                Users = _context.Users.ToDictionary(usr => usr.Id, usr => usr.UserName),
                UserLogs = from u in _context.UserLogs select u
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
