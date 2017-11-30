using System.Collections.Generic;
using System.Linq;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportViewModel
    {
        public Report Report { get; set; }
        public DropdownOptions Options { get; set; }
        public string PlanIds { get; set; }
        public IEnumerable<int> Plans { get; set; }

        public ReportViewModel() { }
        public ReportViewModel(Report report, DropdownOptions options = null)
        {
            Report = report;
            Options = options;
        }
    }
}
