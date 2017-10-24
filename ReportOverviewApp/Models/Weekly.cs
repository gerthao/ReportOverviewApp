using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Weekly : Frequency
    {
        public DayOfWeek DayDue { get; set; }
        public override int Period => 7;

        public override string Name => throw new NotImplementedException();

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            if(selectedDateTime == null)
            {
                return null;
            }
            while(selectedDateTime.DayOfWeek != DayDue)
            {
                selectedDateTime.AddDays(1);
            }
            return selectedDateTime;
        }
    }
}
