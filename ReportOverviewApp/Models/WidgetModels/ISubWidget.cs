using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public interface ISubWidget
    {
        string Topic { get; set; }
        string Description { get; set; }
        Delegate Action { get; set; }
    }
}
