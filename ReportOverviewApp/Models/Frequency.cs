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
        public DateTime? EnsureBusinessDay(DateTime selectedDateTime, bool decrement = true)
        {
            if (selectedDateTime == null) {
                return null;
            }
            while(selectedDateTime.DayOfWeek is DayOfWeek.Saturday || selectedDateTime.DayOfWeek is DayOfWeek.Sunday)
            {
                if (decrement)
                {
                    selectedDateTime = selectedDateTime.AddDays(-1);
                } else
                {
                    selectedDateTime = selectedDateTime.AddDays(1);
                }
            }
            return selectedDateTime;
        }
    }
}
