using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class States
    {
        public enum StateInitials
        {
            AK = 1, AL, AR, AZ, CA, CO, CT, DE, DC, FL, GA, HI, IA, ID, IL, IN, KS, KY, LA, MA, MD, ME, MI, MN, MO, MS, MT, NC, ND, NE, NH, NJ, NM, NV, NY, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VA, VT, WA, WI, WV, WY
        }
        public StateInitials Initials { get; set; }
    }
}
