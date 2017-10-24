using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public abstract class Frequency
    {
        public int Id { get; set; }
        public abstract string Name { get; }
        public abstract int Period { get; }
        public abstract DateTime? GetDeadline(DateTime selectedDateTime);
        public DateTime? EnsureBusinessDay(DateTime selectedDateTime)
        {
            if (selectedDateTime == null) {
                return null;
            }
            while(selectedDateTime.DayOfWeek is DayOfWeek.Saturday || selectedDateTime.DayOfWeek is DayOfWeek.Sunday)
            {
                selectedDateTime = selectedDateTime.AddDays(-1);
            }
            return selectedDateTime;
        }
    }
}
