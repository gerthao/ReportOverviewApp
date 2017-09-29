using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{
    public class ReportDeadline
    {
        public int Id { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        public bool IsFinished { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? FinishedDate { get; set; }

        public bool IsClientNotified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? ClientNotifiedDate { get; set; }

        public bool IsSent { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? SentDate { get; set; }

        public Report Report { get; set; }
        public int ReportId { get; set; }
    }
}
