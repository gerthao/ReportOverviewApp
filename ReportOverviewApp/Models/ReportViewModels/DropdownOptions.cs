using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class DropdownOptions
    {
        public IEnumerable<string> Frequencies { get; set; }
        public IEnumerable<BusinessContact> BusinessContacts { get; set; }
        public IEnumerable<string> BusinessOwners { get; set; }
        public IEnumerable<string> SourceDepartments { get; set; }
        public IEnumerable<Plan> Plans { get; set; }
        public IEnumerable<State> States { get; set; }
    }
}
