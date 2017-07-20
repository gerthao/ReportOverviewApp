using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    [NotMapped]
    public static class ReportEnum
    {
        public enum FrequencyType
        {
            Weekly, BiWeekly, Quarterly, Monthly, Semiannual, Annual
        }
    }
}
