using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Models.ReportViewModels
{
    /// <summary>
    /// This class is the ViewModel for the Reports
    /// </summary>
    public class ReportListViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<ReportDeadline> ReportDeadlines { get; set; }
        public DropdownOptions Options { get; set; }
        public Filters Filters { get; set; }

        private int Pages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; private set; } = 1;
        public const int DefaultPageSize = 100;
        
        public ReportListViewModel(DropdownOptions options = null, Filters filters = null)
        {
            if(options == null)
            {
                options = new DropdownOptions();
            } if(filters == null)
            {
                filters = new Filters();
            }
            Options = options;
            Filters = filters;
        }

        /// <summary>
        ///  GeneratePages method calculates the number of pages needed
        ///  for the number of reports the ReportViewModel contains based
        ///  on the number of Reports per page.
        /// </summary>
        /// <param name="mod">
        ///  Parameter determines how many Reports there are per page, which
        ///  cannot be less than 1.
        /// </param>
        public void GeneratePages(int mod)
        {
            if (Reports == null) return;
            if (mod <= 0) mod = DefaultPageSize;
            PageSize = mod;
            Pages = Reports.Count() / PageSize;
            if (Reports.Count() % PageSize > 0) Pages++;
        }
        public int PagesCount() => Pages;

        public IEnumerable<Report> DisplayPage(int index)
        {
            if (Reports == null) return null;
            if(index <= 0) index = 1;
            CurrentPage = index;
            return Reports.Skip((CurrentPage - 1)*PageSize).Take(PageSize);
        }
        
        public string DisplayDateTimeAsDate(DateTime? date)
        {
            if (date == null || !date.HasValue) return null;
            return date.Value.ToString("MM/dd/yyy");
        }
        public void ApplyFilters()
        {
            HandleBusinessContact();
            HandleBusinessOwner();
            HandleDates();
            HandleFrequency();
            HandleSearch();
            HandleSourceDepartment();
            HandleStateAndPlan();
            HandleSort();
        }
        private void HandleBusinessContact()
        {
            if (!String.IsNullOrEmpty(Filters.BusinessContact))
            {
                Reports = Reports.Where(r => r != null && r.BusinessContact == Filters.BusinessContact);
            }
        }
        private void HandleBusinessOwner()
        {
            if (!String.IsNullOrEmpty(Filters.BusinessOwner))
            {
                Reports = Reports.Where(r => r != null && r.BusinessOwner == Filters.BusinessOwner);
            }
        }
        private void HandleSourceDepartment()
        {
            if (!String.IsNullOrEmpty(Filters.SourceDepartment))
            {
                Reports = Reports.Where(r => r != null && r.SourceDepartment == Filters.SourceDepartment);
            }
        }

        private void HandleFrequency()
        {
            if (!String.IsNullOrEmpty(Filters.Frequency))
            {
                Reports = Reports.Where(r => r != null && r.Frequency != null && r.Frequency == Filters.Frequency);
            }
        }
        private void HandleSort()
        {
            if (!String.IsNullOrEmpty(Filters.Column))
            {
                switch (Filters.Column)
                {
                    case "Id":
                        Reports = Reports.OrderBy(report => report.Id);
                        break;
                    case "Name":
                        Reports = Reports.OrderBy(report => report.Name);
                        break;
                    case "Deadline":
                        Reports = Reports.OrderBy(report => report.CurrentDeadline());
                        break;
                    //case "Finished On":
                    //    viewModel.Reports = viewModel.Reports.OrderBy(report => report.FinishedDate);
                    //    break;
                    //case "Notified On":
                    //    viewModel.Reports = viewModel.Reports.OrderBy(report => report.ClientNotifiedDate);
                    //    break;
                    //case "Sent On":
                    //    viewModel.Reports = viewModel.Reports.OrderBy(report => report.SentDate);
                    //break;
                    default:
                        Reports = Reports.OrderBy(report => report.Id);
                        break;
                }

            }
        }
        private void HandleDates()
        {
            IEnumerable<(Report report, ReportDeadline reportDeadline)> list = Reports.Select(r => (r, r.Deadlines.Any()? r.Deadlines.OrderByDescending(rd => rd.Deadline).First() : null));
            list = list.Where(r => r.reportDeadline != null);
            if (Filters.BeginString != null)
            {
                DateTime beginDate;
                if (DateTime.TryParse(Filters.BeginString, out beginDate))
                {
                    Filters.Begin = beginDate;
                    list = list.Where(r => r.reportDeadline.Deadline >= beginDate);
                }
            }
            if (Filters.EndString != null)
            {
                DateTime endDate;
                if (DateTime.TryParse(Filters.EndString, out endDate))
                {
                    Filters.End = endDate;
                    list = list.Where(r => r.reportDeadline.Deadline <= endDate);
                }
            }
            Reports = list.Select(r => r.report);
        }
        private void HandleStateAndPlan()
        {
            if (!String.IsNullOrEmpty(Filters.State))
            {
                Reports = Reports.Where(r => r != null && r.State != null && r.State.Equals(Filters.State));
            }
            if (!String.IsNullOrEmpty(Filters.Plan))
            {
                Reports = Reports.Where(r => r != null && r.GroupName != null && r.GroupName.Equals(Filters.Plan));
            }
        }
        private void HandleSearch()
        {
            if (!String.IsNullOrEmpty(Filters.Search))
            {
                Reports = Reports.Where(r => r != null && r.Name != null && r.Name.ToLowerInvariant().Contains(Filters.Search.ToLowerInvariant()));
            }
        }
    }
}
