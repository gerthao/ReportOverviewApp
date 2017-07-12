using ReportOverviewApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    /// <summary>
    ///  Interface used in part of the IWidget interface to
    ///  display numerical or statistical data
    /// </summary>
    public interface ISubWidget
    {
        string Topic { get; set; }
        string Description { get; set; }
        Func<ApplicationDbContext, int> Action { get; set; }
    }
}
