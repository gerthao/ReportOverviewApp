using System.Collections.Generic;
using System.Linq;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportViewModel
    {
        public Report Report { get; set; }
        public DropdownOptions Options { get; set; }
        public ReportViewModel(ReportListViewModel reportListViewModel)
        {
            Options = reportListViewModel.Options;
        }
    }
}
