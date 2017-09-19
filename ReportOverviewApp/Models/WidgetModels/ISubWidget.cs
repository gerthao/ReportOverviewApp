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
        int ID { get; set; }
        string Topic { get; set; }
        string Description { get; set; }
    }
}
