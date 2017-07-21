using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public interface IWidget
    {
        int ID { get; set; }
        IWidgetOptions Options {get; set;}
        string Color { get; set; }
        string Header { get; set; }
        ISubWidget Body { get; set; }
        [ForeignKey("ISubWidget")]
        int SubWidgetID { get; set; }
        string Footer { get; set; }

    }
}
