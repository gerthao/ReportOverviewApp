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
    }
}
