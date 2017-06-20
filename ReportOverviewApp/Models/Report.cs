using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }

        [Display(Name = "Due Date"), DataType(DataType.Date)]
        public DateTime DateDue { get; set; }
        [Display(Name = "Report Finished"), DataType(DataType.DateTime)]
        public DateTime? DateDone { get; set; }
        [Display(Name = "Client Notified"), DataType(DataType.DateTime)]
        public DateTime? DateClientNotified { get; set; }
        [Display(Name = "Report Sent"), DataType(DataType.DateTime)]
        public DateTime? DateSent { get; set; }
        public bool ClientNotified { get; set; }
        public bool Sent { get; set; }

    }
}
