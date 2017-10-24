using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BusinessOwnerId { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
