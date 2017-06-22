using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    interface IWidget
    {
        int ID { get; set; }
        
        string color { get; set; }
        string header { get; set; }
        ISubWidget body { get; set; }
        string footer { get; set; }
    }
}
