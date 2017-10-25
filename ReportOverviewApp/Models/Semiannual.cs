using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Semiannual : Frequency
    {
        public override string Name => "Semiannual";

        public override int Period => 6;

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            if(selectedDateTime == null)
            {
                selectedDateTime = DateTime.Now;
            }
            DateTime deadline = DateTime.Today;
            return null;
        }
    }
}
