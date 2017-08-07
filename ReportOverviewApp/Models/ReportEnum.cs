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
        public enum StateType
        {
            AK = 1, AL, AR, AZ, CA, CO, CT, DE, DC, FL, GA, HI, IA, ID, IL, IN, KS, KY, LA, MA, MD, ME, MI, MN, MO, MS, MT, NC, ND, NE, NH, NJ, NM, NV, NY, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VA, VT, WA, WI, WV, WY
        }
    }
}
