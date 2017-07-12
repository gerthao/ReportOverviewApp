using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.WidgetModels
{
    /// <summary>
    ///  This class contains the options for the Widget
    ///  class
    /// </summary>
    public class WidgetOptions : IWidgetOptions
    {
        //public Dictionary<String, Delegate> Options { get; set; }
        public List<string> Options { get; set; }

        public WidgetOptions()
        {
            Options = new List<String>();
        }
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
