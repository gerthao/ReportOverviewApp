﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ReportOverviewApp.Models
{
    /// <summary>
    /// Class used to convert a specific excel document to JSON
    /// </summary>
    [NotMapped]
    public class ReportJsonData
    {
        private DateTime ExcelBaseDate = new DateTime(month: 12, day: 30, year: 1899);
        [JsonProperty("ID")]
        public int Id { get; set; }
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
        [JsonProperty("FREQUENCY")]
        public string Frequency { get; set; }
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
        [JsonProperty("DELIVER_TO")]
        public string DeliverTo { get; set; }
        [JsonProperty("EFFECTIVE_DATE")]
        public double? EffectiveDate { get; set; }
        [JsonProperty("TERMINATION_DATE")]
        public double? TerminationDate { get; set; }
        [JsonProperty("GROUP_NAME")]
        public string GroupName { get; set; }
        [JsonProperty("STATE")]
        public string State { get; set; }
        [JsonProperty("WW_GROUP_ID")]
        public string WwGroupId { get; set; }
        [JsonProperty("REPORT_PATH")]
        public string ReportPath { get; set; }
        //[JsonProperty("OTHER_DEPARTMENT")]
        public bool OtherDepartment { get; set; }
        [JsonProperty("SOURCE_DEPARTMENT")]
        public string SourceDepartment { get; set; }
        [JsonProperty("QUALITY_INDICATOR")]
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

        private State DefaultState = new State()
        {
            Name = "N/A",
            PostalAbbreviation = "N/A",
            Type = "N/A"
        };

        private DateTime? ToDate(double? days)
        {
            if (days == null) return null;
            return ExcelBaseDate.AddDays(days.Value);
        }

        public BusinessContact GetBusinessContacts()
        {
            return null;
        }

        public (Report, Plan) ToReport()
        {
            Report report = new Report();
            report.ReportPlanMapping = new List<ReportPlanMap>();
            Plan plan = new Plan();
            if (String.IsNullOrEmpty(BusinessContact))
            {
                report.BusinessContact = null;
            } else
            {
                report.BusinessContact = new BusinessContact()
                {
                    Name = BusinessContact?.Trim(),
                    BusinessOwner = "Client Engagement"
                };
            }
            if (String.IsNullOrEmpty(SourceDepartment))
            {
                report.SourceDepartment = null;
            } else
            {
                report.SourceDepartment = SourceDepartment.Trim();
            }
            if (!String.IsNullOrEmpty(State))
            {
                plan.State = new State() { PostalAbbreviation = State.Trim().ToUpper() };
            }
            else
            {
                plan.State = DefaultState;
            }
            if (String.IsNullOrEmpty(GroupName))
            {
                GroupName = $"N/A ({State})";
            }
            plan.Name = GroupName?.Replace("-", " - ").Replace("  ", " ").Trim();
            plan.WindwardId = WwGroupId?.Trim();
            report.Name = Name?.Trim();
            report.DueDate1 = ToDate(DueDate1);
            report.DueDate2 = ToDate(DueDate2);
            report.DueDate3 = ToDate(DueDate3);
            report.DueDate4 = ToDate(DueDate4);
            report.DateAdded = ToDate(DateAdded);
            report.EffectiveDate = ToDate(EffectiveDate);
            report.TerminationDate = ToDate(TerminationDate);
            report.SystemRefreshDate = ToDate(SystemRefreshDate);
            report.Frequency = Frequency?.Trim();
            report.DayDue = DayDue;
            report.DeliveryFunction = DeliveryFunction;
            report.DeliveryMethod = DeliveryMethod;
            report.DeliverTo = DeliverTo;
            report.WorkInstructions = WorkInstructions?.Trim();
            report.DaysAfterQuarter = DaysAfterQuarter;
            report.FolderLocation = FolderLocation?.Trim();
            report.ReportType = ReportType;
            report.RunWith = RunWith;
            report.Notes = Notes?.Trim();
            report.DaysAfterQuarter = DaysAfterQuarter;
            //report.Plan = GroupName?.Trim();
            //report.State = State?.Trim();
            report.ReportPath = ReportPath?.Trim();
            report.IsQualityIndicator = QualityIndicator;
            report.ERSReportLocation = ERSReportLocation?.Trim();
            report.ERRStatus = ERRStatus;
            report.LegacyReportID = LegacyReportID;
            report.LegacyReportIDR2 = LegacyReportIDR2;
            report.ERSReportName = ERSReportName?.Trim();
            report.OtherReportLocation = OtherReportLocation?.Trim();
            report.OtherReportName = OtherReportName?.Trim();

            report.Name = $"{Id}:{report.Name}";
            //if(plan != null)
            //{
            //    report.ReportPlanMapping.Add(new ReportPlanMap()
            //    {
            //        Report = report,
            //        Plan = plan
            //    });
            //}
            

            return (report, plan);
        }
    }
}
