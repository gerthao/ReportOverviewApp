using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Annual : Frequency
    {
        public override int Period => 1;
        public int Month { get; set; }
        public int Day { get; set; }

        public override string Name => "Annual";

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            if(selectedDateTime == null)
            {
                selectedDateTime = DateTime.Today;
            }
            DateTime deadline = new DateTime(year: selectedDateTime.Year, month: Month, day: Day);
            while(deadline < selectedDateTime)
            {
                deadline.AddYears(1);
            }
            return EnsureBusinessDay(deadline);
        }
    }
}
