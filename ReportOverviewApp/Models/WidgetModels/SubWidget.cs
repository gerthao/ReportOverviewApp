using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class SubWidget : ISubWidget
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public Func<ApplicationDbContext, int> Action { get; set; }
    }
}
