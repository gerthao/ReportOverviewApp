using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class State
    {
        public int Id { get; set; }
        [StringLength(32), JsonProperty("abbreviation")]
        public string PostalAbbreviation { get; set; }
        [JsonProperty("name"), StringLength(64)]
        public string Name { get; set; }
        [StringLength(64)]
        [JsonProperty("type")]
        public string Type { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public bool ContainsPlans { get => Plans != null && Plans.Any(); }
        
        public enum StateInitials
        {
            AK = 1, AL, AR, AZ, CA, CO, CT, DE, DC, FL, GA, HI, IA, ID, IL, IN, KS, KY, LA, MA, MD, ME, MI, MN, MO, MS, MT, NC, ND, NE, NH, NJ, NM, NV, NY, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VA, VT, WA, WI, WV, WY
        }
        //public string PostalAbbreviationToName(string abbreviation)
        //{
        //    switch (abbreviation)
        //    {
        //        case "AK":
        //            return "Alaska";
        //        case "AL":
        //            return "Alabama";
        //        case "AR":
        //            return "Arkansas";
        //        case "AZ":
        //            return "Arizona";
        //        case "CA":
        //            return "California";
        //        case "CO":
        //            return "Colorado";
        //        case "CT":
        //            return "Connecticut";
        //        case "DE":
        //            return "Deleware";
        //        case "DC":
        //            return "Washington DC";
        //        case "FL":

        //    }
        //}
    }
}
