using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class ReportPlanMap
    {
        public int Id { get; set; }
        public Report Report { get; set; }
        public int ReportId { get; set; }
        public Plan Plan { get; set; }
        public int PlanId { get; set; }
    }
}
