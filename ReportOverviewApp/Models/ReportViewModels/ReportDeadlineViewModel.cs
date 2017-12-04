using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportDeadlineViewModel
    {
        public DateTime? Deadline { get; set; }
        public IEnumerable<ReportDeadline> ReportDeadlines { get; set; }
        public bool HasDeadline()
        {
            return Deadline != null && Deadline.HasValue;
        }
        public IEnumerable<ReportDeadline> DisplayReports()
        {
            if (HasDeadline())
            {
                return ReportDeadlines.OrderBy(rd => rd.Report.Name).Where(rd => rd.Deadline == Deadline.Value);
            }
            return ReportDeadlines.OrderBy(rd => rd.Report.Name);
        }
        public string DisplayDeadline()
        {
            if (HasDeadline())
            {
                if(Deadline == DateTime.Today)
                {
                    return Deadline.Value.ToShortDateString() + " (Today)";
                } else if (Deadline == DateTime.Today.AddDays(-1))
                {
                    return Deadline.Value.ToShortDateString() + " (Yesterday)";
                } else if (Deadline == DateTime.Today.AddDays(1))
                {
                    return Deadline.Value.ToShortDateString() + " (Tomorrow)";
                } else
                {
                    return Deadline.Value.ToShortDateString();
                }
            }
            return "All Dates";
        }
        //public string GetStatus(ReportDeadline reportDeadline)
        //{
        //    string messageFinished = "", clientNotifiedMessage = "", sentMessage = "";
        //    if (HasDeadline() && reportDeadline.Deadline == Deadline)
        //    {
        //        if (reportDeadline.IsSent)
        //        {
        //            sentMessage = CheckStatus("Sent", reportDeadline.SentDate);
        //        }
        //        else if(reportDeadline.IsClientNotified)
        //        {
        //            clientNotifiedMessage = CheckStatus("Notified", reportDeadline.ClientNotifiedDate);
        //        }
        //        else if (reportDeadline.IsFinished)
        //        {
        //            messageFinished = CheckStatus("Finished", reportDeadline.RunDate);
        //        }
        //        else
        //        {
        //            if(Deadline < DateTime.Today)
        //            {
        //                return "Past Due";
        //            }
        //            else (Deadline >= DateTime.Today)
        //            {
        //                return "In Progress";
        //            }
        //        }
        //        return 
        //    }
        //}
        private string CheckStatus(string message, DateTime? dateTime)
        {
            if(dateTime > Deadline)
            {
                return $"{message} (Late)";
            }
            return message;
        }
    }
}
