using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    [ViewComponent(Name ="TimeViewComponent")]
    public class TimeViewComponent : ViewComponent
    {
        private string time;
        public IViewComponentResult Invoke()
        {
            time = DateTime.Now.ToString("h:mm:ss");
            return Content($"Current Time: {time}");
        }
    }
}
