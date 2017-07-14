using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models
{

    public class Report
    {
        //public enum FrequencyType
        //{
        //    Weekly, Biweekly, Quarterly, Monthly, Semiannual, Annual
        //}

        public int ID { get; set; }
        
        public bool Done { get; set; }
        //[JsonProperty("FREQUENCY")]
        //public FrequencyType Frequency { get; set; }

        [DataType(DataType.Date), Required, DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime DateDue { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateDone { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateClientNotified { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateSent { get; set; }
        public bool ClientNotified { get; set; }
        public bool Sent { get; set; }

        [JsonProperty("REPORT_NAME")]
        public string Name { get; set; }
        [JsonProperty("BUSINESS_CONTACT")]
        public string BusinessContact { get; set; }
        [JsonProperty("BUSINESS_OWNER")]
        public string BusinessOwner { get; set; }
        [JsonProperty("DUE_DATE_1")]
        public string DueDate1 { get; set; }
        [JsonProperty("DUE_DATE_2")]
        public string DueDate2 { get; set; }
        [JsonProperty("DUE_DATE_3")]
        public string DueDate3 { get; set; }
        [JsonProperty("DUE_DATE_4")]
        public string DueDate4 { get; set; }
        [JsonProperty("DAY_DUE")]
        public string DayDue { get; set; }
        [JsonProperty("DELIVERY_FUNCTION")]
        public string DeliveryFunction { get; set; }
        [JsonProperty("WORK_INSTRUCTIONS")]
        public string WorkInstructions { get; set; }
        [JsonProperty("NOTES")]
        public string Notes{ get; set; }
        [JsonProperty("DAYS_AFTER_QUARTER")]
        public int? DaysAfterQuarter { get; set; }
        [JsonProperty("FOLDER_LOCATION")]
        public string FolderLocation { get; set; }
        [JsonProperty("REPORT_TYPE")]
        public string ReportType { get; set; }
        [JsonProperty("RUN_WITH")]
        public string RunWith { get; set; }
        [JsonProperty("DELIVERY_METHOD")]
        public string DeliveryMethod { get; set; }
        [JsonProperty("DELIVERY_TO")]
        public string DeliveryTo { get; set; }
        [JsonProperty("EFFECTIVE_DATE")]
        public string EffectiveDate { get; set; }
        [JsonProperty("TERMINATION_DATE")]
        public string TerminationDate { get; set; }
        [JsonProperty("GROUP_NAME")]
        public string GroupName { get; set; }
        [JsonProperty("STATE")]
        public string State { get; set; }
        [JsonProperty("REPORT_PATH")]
        public string ReportPath { get; set; }
        [JsonProperty("Other Department")]
        public string OtherDepartment { get; set; }
        [JsonProperty("Quality Indicator")]
        public string QualityIndicator { get; set; }
        [JsonProperty("ERS_REPORT_LOCATION")]
        public string ERSReportLocation { get; set; }
        [JsonProperty("ERR_STATUS")]
        public int? ERR_STATUS { get; set; }
        [JsonProperty("DATE_ADDED")]
        public string DateAdded { get; set; }
        [JsonProperty("SYSTEM_REFRESH_DATE")]
        public string SystemRefreshDate { get; set; }
        [JsonProperty("LEGACY_REPORT_ID")]
        public int? LegacyReportID { get; set; }
        [JsonProperty("LEGACY_REPORT_ID_R2")]
        public int? LegacyReportIDR2 { get; set; }
        [JsonProperty("ERS_REPORT_NAME")]
        public string ERSReportName { get; set; }
        [JsonProperty("OTHER_REPORT_LOCATION")]
        public string OtherReportLocation { get; set; }
        [JsonProperty("OTHER_REPORT_NAME")]
        public string OtherReportName { get; set; }

        public bool IsPastDue() => DateDue != null && DateDue > DateTime.Now;
        public bool IsPastDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate < DateDue;
        public bool IsDue() => DateDue != null && DateDue <= DateTime.Now;
        public bool IsDue(DateTime SelectedDate) => DateDue != null && SelectedDate != null && SelectedDate >= DateDue;
        public bool IsDone() => DateDone != null;
        public bool HasBeenSent() => DateSent != null;
        public bool HasBeenDone() => DateDone != null;
        public bool HasBeenNotified() => DateClientNotified != null;
    }
}
