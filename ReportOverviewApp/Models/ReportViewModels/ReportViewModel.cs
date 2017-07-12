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
        private int Pages;
        private int PageSize;
        public const int DEFAULT_PAGE_SIZE = 20;


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
            if (mod <= 0) mod = DEFAULT_PAGE_SIZE;
            PageSize = mod;
            int Pages = Reports.Count() / PageSize;
            if (Reports.Count() % PageSize > 0) Pages++;
        }

        public int PagesCount()
        {
            return Pages;
        }

        public IEnumerable<Report> DisplayPage(int index)
        {
            return Reports.Take(index);
        }
        //public string DisplayMessage(DateTime? date)
        //{
        //    return String.Format("{0:MM/dd}", date);
        //}
    }
}
