using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ReportOverviewApp.Models.DA
{
    public class DAContact
    {
        [Display(Name = "ContactID")]
        public int ID { get; set; }
        [JsonProperty("Email"), Display(Name = "Email")]
        public string Email { get; set; }
        [JsonProperty("EmailStatus"), Display(Name = "EmailStatus")]
        public bool Status { get; set; }
        [JsonProperty("PurchaserID"), Display(Name = "PurchaserID")]
        public int PurchaserID { get; set; }
    }
}
