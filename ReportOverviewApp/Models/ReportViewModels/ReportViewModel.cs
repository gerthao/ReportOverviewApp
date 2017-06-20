using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        public Report ReportToken { get; set; }
    }
}
