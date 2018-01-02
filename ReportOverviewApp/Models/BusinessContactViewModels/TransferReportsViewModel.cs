using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.BusinessContactViewModels
{
    public class TransferReportsViewModel
    {
        public int First { get; set; }
        public int Second { get; set; }
        public List<BusinessContact> BusinessContacts { get; set; }
        public List<int> FirstReportIds { get; set; }
        public List<int> SecondReportIds { get; set; }
    }
}
