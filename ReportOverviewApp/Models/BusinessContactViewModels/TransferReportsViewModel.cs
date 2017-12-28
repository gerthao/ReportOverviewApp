using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.BusinessContactViewModels
{
    public class TransferReportsViewModel
    {
        public BusinessContact Owner { get; set; }
        public BusinessContact Recipient { get; set; }
        public IEnumerable<BusinessContact> BusinessContacts { get; set; }
        public IEnumerable<int> ReportIds { get; set; }
    }
}
