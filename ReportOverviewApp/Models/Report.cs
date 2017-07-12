using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{

    public class Report
    {
        public enum FrequencyType
        {
            Weekly, Biweekly, Quarterly, Monthly, Semiannual, Annual
        }

        public int ID { get; set; }
        
        public bool Done { get; set; }
        [Required]
        public FrequencyType Frequency { get; set; }

        [DataType(DataType.Date), Required, DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime DateDue { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateDone { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateClientNotified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateSent { get; set; }
        public bool ClientNotified { get; set; }
        public bool Sent { get; set; }


        public string Name { get; set; }




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
