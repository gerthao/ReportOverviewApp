using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    /// <summary>
    ///  This class is used in the Widget class and
    ///  determines the numerical data that is displayed.
    /// </summary>
    public class SubWidget : ISubWidget
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public Func<ApplicationDbContext, int> Action { get; set; }
    }
}
