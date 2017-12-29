using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.BusinessContactViewModels
{
    public class TransferReportsViewModel
    {
        public BusinessContact First { get; set; }
        public BusinessContact Second { get; set; }
        public IEnumerable<BusinessContact> BusinessContacts { get; set; }
        public IEnumerable<int> FirstReportIds { get; set; }
        public IEnumerable<int> SecondReportIds { get; set; }
    }
}
