using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReportOverviewApp.Models
{
    public class Plan
    {
        [JsonProperty("WW_GROUP_ID")]
        public int PlanID { get; set; }
        [JsonProperty("GROUP_NAME")]
        public string PlanName { get; set; }
        [JsonProperty("STATE")]
        public string State { get; set; }
    }
}
