﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class Quarterly : Frequency
    {
        public bool UsesFiscalYear { get; set; }
        public int DaysAfterQuarter { get; set; }

        public override int Period => 3;

        public override string Name => "Quarterly";

        public override DateTime? GetDeadline(DateTime selectedDateTime)
        {
            DateTime firstQuarter = new DateTime(year: DateTime.Now.Year, month: 3, day: 31);
            return null;
        }

    }
}
