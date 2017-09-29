using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class Filters
    {
        public string BusinessContact { get; set; }
        public string BusinessOwner { get; set; }
        public string SourceDepartment { get; set; }
        public string Frequency { get; set; }
        public string Search { get; set; }
        public string Column { get; set; }
        public string State { get; set; }
        public string Plan { get; set; }
        public DateTime? FinishedBegin { get; set; }
        public DateTime? FinishedEnd { get; set; }
        public DateTime? NotifiedBegin { get; set; }
        public DateTime? NotifiedEnd { get; set; }
        public DateTime? SentBegin { get; set; }
        public DateTime? SentEnd { get; set; }
        public string BeginString { get; set; }
        public string EndString { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
