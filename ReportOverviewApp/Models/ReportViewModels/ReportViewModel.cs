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
    public class ReportViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<string> Frequencies { get; set; }
        public IEnumerable<string> BusinessContacts { get; set; }
        public IEnumerable<string> BusinessOwners { get; set; }
        public IEnumerable<string> SourceDepartments { get; set; }
        public IEnumerable<string> Plans { get; set; }
        public IEnumerable<string> States { get; set; }
        public string BusinessContact { get; set; }
        public string BusinessOwner { get; set; }
        public string SourceDepartment { get; set; }
        public string Frequency { get; set; }
        private int Pages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; private set; } = 1;
        public const int DEFAULT_PAGE_SIZE = 100;
        public string Search{ get; set; }
        public string Column { get; set; }
        public string State { get; set; }
        public string Plan { get; set; }
        public DateTime? DoneBegin { get; set; }
        public DateTime? DoneEnd { get; set; }
        public DateTime? NotifiedBegin { get; set; }
        public DateTime? NotifiedEnd { get; set; }
        public DateTime? SentBegin { get; set; }
        public DateTime? SentEnd { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

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
            if (mod <= 0) mod = DEFAULT_PAGE_SIZE;
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
    }
}
