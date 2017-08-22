using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.DA
{
    public class DAReport
    {
        [JsonProperty("PurchaserID"), Display(Name = "PurchaserID")]
        public int ID { get; set; }
        [JsonProperty("PurchaserName"), Display(Name = "PurchaserName")]
        public string Name { get; set; }
        [JsonProperty("HRAdminFirst"), Display(Name = "HRAdminFirst")]
        public string HRAdminFirstName { get; set; }
        [JsonProperty("HRAdminLast"), Display(Name = "HRAdminLast")]
        public string HRAdminLastName { get; set; }
        //public IEnumerable<DAContact> Contacts { get; set; }
        [JsonProperty("AgeDep"), Display(Name = "AgeDep")]
        public int DependentAge { get; set; }
        [JsonProperty("AgeStudent"), Display(Name = "AgeStudent")]
        public int StudentAge { get; set; }
        [JsonProperty("ReportSent"), Display(Name = "ReportSent")]
        public bool Sent { get; set; }
    }
}
