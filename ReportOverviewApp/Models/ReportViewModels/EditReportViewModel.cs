using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class EditReportViewModel
    {
        public Report Report { get; set; }
        public IEnumerable<string> Frequencies { get; set; }
        public IEnumerable<string> GroupNames { get; set; }
        public IEnumerable<string> States { get; set; }
        public IEnumerable<string> BusinessContacts { get; set; }
        public IEnumerable<string> BusinessOwners { get; set; }
        public IEnumerable<string> SourceDepartments { get; set; }

        public EditReportViewModel(ReportViewModel reportViewModel)
        {
            Frequencies = reportViewModel.Reports.Select(r => r.Frequency).Distinct();
            
        }
    }
}
