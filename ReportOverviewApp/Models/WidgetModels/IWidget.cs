using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public interface IWidget
    {
        int ID { get; set; }
        WidgetOptions Options {get; set;}
        string Color { get; set; }
        string Header { get; set; }
        ISubWidget Body { get; set; }
        string Footer { get; set; }

    }
}
