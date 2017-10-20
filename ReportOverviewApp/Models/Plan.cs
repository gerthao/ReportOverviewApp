using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Plan
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(64)]
        public string WindwardId { get; set; }
        public State State { get; set; }
        public int StateId { get; set; }

        public virtual ICollection<ReportPlanMap> ReportPlanMapping { get; set; }
        public bool HasTermedReports { get => ReportPlanMapping != null && ReportPlanMapping.Select(rpm => rpm.Report).Any(r => r.IsTermed(null)); }
        public bool HasActiveReports { get => ReportPlanMapping != null && ReportPlanMapping.Select(rpm => rpm.Report).Any(r => !r.IsTermed(null)); }
    }
}
