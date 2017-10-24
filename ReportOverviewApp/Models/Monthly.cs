using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Monthly : Frequency
    {
        public int DayOfMonth { get; set; }

        public override int Period => 1;

        public override string Name => throw new NotImplementedException();

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            if(selectedDateTime == null)
            {
                selectedDateTime = DateTime.Today;
            }
            DateTime Deadline = new DateTime(year: selectedDateTime.Year, month: selectedDateTime.Month, day: DayOfMonth);
            return EnsureBusinessDay(Deadline);
        }
    }
}
