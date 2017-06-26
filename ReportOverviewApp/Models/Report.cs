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

        [Display(Name = "Deadline"), DataType(DataType.Date)]
        public DateTime DateDue { get; set; }
        public DateTime? DateDone { get; set; }
        public DateTime? DateClientNotified { get; set; }
        public DateTime? DateSent { get; set; }
        public bool ClientNotified { get; set; }
        public bool Sent { get; set; }

        public bool IsPastDue() => DateDue != null && DateDue > DateTime.Now;
        public bool IsPastDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate < DateDue;
        public bool IsDue() => DateDue != null && DateDue <= DateTime.Now;
        public bool IsDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate >= DateDue;
        public bool IsDone() => DateDone != null;
        public bool HasBeenSent() => DateSent != null;
        public bool HasBeenDone() => DateDone != null;
        public bool HasBeenNotified() => DateClientNotified != null;
    }
}
