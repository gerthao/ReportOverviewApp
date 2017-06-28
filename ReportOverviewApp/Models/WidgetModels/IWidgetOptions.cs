using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public interface IWidgetOptions : IEnumerable<string>
    {
        List<string> Options { get; set; }
    }
}
