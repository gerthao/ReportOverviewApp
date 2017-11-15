using System.Collections.Generic;
using System.Linq;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportViewModel
    {
        public Report Report { get; set; }
        public DropdownOptions Options { get; set; }

        public ReportViewModel() { }
        public ReportViewModel(Report report, DropdownOptions options = null)
        {
            Report = report;
            Options = options;
        }
    }
}
