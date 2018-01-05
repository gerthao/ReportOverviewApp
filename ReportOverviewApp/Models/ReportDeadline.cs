using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    [JsonObject(IsReference = true)]
    public class ReportDeadline
    {
        public int Id { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        public bool HasRun { get => RunDate != null; }
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? RunDate { get; set; }

        public bool IsApproved { get => ApprovalDate != null; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovalDate { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? SentDate { get; set; }
        public bool IsSent { get => SentDate != null; }
        //[JsonIgnore, IgnoreDataMember]
        public Report Report { get; set; }

        public int ReportId { get; set; }
    }
}
