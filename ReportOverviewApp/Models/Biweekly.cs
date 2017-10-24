using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Biweekly : Frequency
    {
        public int StartingDay{ get; set; }

        public override int Period => 14;

        public override string Name => throw new NotImplementedException();

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            throw new NotImplementedException();
        }
    }
}
