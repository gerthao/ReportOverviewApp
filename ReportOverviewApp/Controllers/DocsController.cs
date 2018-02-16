using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ReportOverviewApp.Controllers
{
    
    public class DocsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Plans()
        {
            return View();
        }
        public IActionResult Deadlines()
        {
            return View();
        }
        public IActionResult BusinessContacts()
        {
            return View();
        }
    }
}