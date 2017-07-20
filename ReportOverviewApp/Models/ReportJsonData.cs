using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportOverviewApp.Models
{
    [NotMapped]
    public class ReportJsonData
    {
        [JsonProperty("REPORT_NAME")]
        public string Name { get; set; }
        [JsonProperty("BUSINESS_CONTACT")]
        public string BusinessContact { get; set; }
        [JsonProperty("BUSINESS_OWNER")]
        public string BusinessOwner { get; set; }
        [JsonProperty("DUE_DATE_1")]
        public double? DueDate1 { get; set; }
        [JsonProperty("DUE_DATE_2")]
        public double? DueDate2 { get; set; }
        [JsonProperty("DUE_DATE_3")]
        public double? DueDate3 { get; set; }
        [JsonProperty("DUE_DATE_4")]
        public double? DueDate4 { get; set; }
        [JsonProperty("FREQUENCY"), JsonConverter(typeof(StringEnumConverter))]
        public ReportEnum.FrequencyType Frequency { get; set; }
        [JsonProperty("DAY_DUE")]
        public string DayDue { get; set; }
        [JsonProperty("DELIVERY_FUNCTION")]
        public string DeliveryFunction { get; set; }
        [JsonProperty("WORK_INSTRUCTIONS")]
        public string WorkInstructions { get; set; }
        [JsonProperty("NOTES")]
        public string Notes { get; set; }
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
        public double? EffectiveDate { get; set; }
        [JsonProperty("TERMINATION_DATE")]
        public double? TerminationDate { get; set; }
        [JsonProperty("GROUP_NAME")]
        public string GroupName { get; set; }
        [JsonProperty("STATE")]
        public string State { get; set; }
        [JsonProperty("REPORT_PATH")]
        public string ReportPath { get; set; }
        [JsonProperty("Other Department")]
        public bool OtherDepartment { get; set; }
        [JsonProperty("Source Department")]
        public string SourceDepartment { get; set; }
        [JsonProperty("Quality Indicator")]
        public bool QualityIndicator { get; set; }
        [JsonProperty("ERS_REPORT_LOCATION")]
        public string ERSReportLocation { get; set; }
        [JsonProperty("ERR_STATUS")]
        public int? ERRStatus { get; set; }
        [JsonProperty("DATE_ADDED")]
        public double? DateAdded { get; set; }
        [JsonProperty("SYSTEM_REFRESH_DATE")]
        public double? SystemRefreshDate { get; set; }
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

        private DateTime? ToDate(double? days)
        {
            if (days == null) return null;
            return new DateTime().AddDays(days.Value);
        }

        public Report ToReport()
        {
            Report report = new Report();
            if(report == null)
            {
                report = new Report();
            }

            report.Name = Name;
            report.BusinessContact = BusinessContact;
            report.BusinessOwner = BusinessOwner;
            report.DueDate1 = ToDate(DueDate1);
            report.DueDate2 = ToDate(DueDate2);
            report.DueDate3 = ToDate(DueDate3);
            report.DueDate4 = ToDate(DueDate4);
            report.DateAdded = ToDate(DateAdded);
            report.EffectiveDate = ToDate(EffectiveDate);
            report.TerminationDate = ToDate(TerminationDate);
            report.SystemRefreshDate = ToDate(SystemRefreshDate);
            report.Frequency = Frequency;
            report.DayDue = DayDue;
            report.DeliveryFunction = DeliveryFunction;
            report.DeliveryMethod = DeliveryMethod;
            report.DeliveryTo = DeliveryTo;
            report.WorkInstructions = WorkInstructions;
            report.DaysAfterQuarter = DaysAfterQuarter;
            report.FolderLocation = FolderLocation;
            report.ReportType = ReportType;
            report.RunWith = RunWith;
            report.Notes = Notes;
            report.DaysAfterQuarter = DaysAfterQuarter;
            report.GroupName = GroupName;
            report.State = State;
            report.ReportPath = ReportPath;
            report.OtherDepartment = OtherDepartment;
            report.SourceDepartment = SourceDepartment;
            report.QualityIndicator = QualityIndicator;
            report.ERSReportLocation = ERSReportLocation;
            report.ERRStatus = ERRStatus;
            report.LegacyReportID = LegacyReportID;
            report.LegacyReportIDR2 = LegacyReportIDR2;
            report.ERSReportName = ERSReportName;
            report.OtherReportLocation = OtherReportLocation;
            report.OtherReportName = OtherReportName;

            return report;
        }
    }
}
