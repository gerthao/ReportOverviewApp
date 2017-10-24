using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class BusinessOwner
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public virtual ICollection<BusinessContact> BusinessContacts{get; set;}
    }
}
