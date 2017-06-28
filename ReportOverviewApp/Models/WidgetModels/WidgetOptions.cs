using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    public class WidgetOptions : IWidgetOptions
    {
        //public Dictionary<String, Delegate> Options { get; set; }
        public List<string> Options { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            return Options.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Options.GetEnumerator();
        }
    }
}
