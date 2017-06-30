using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<Report> Reports { get; set; }
        private int Pages;
        private int PageSize;

        public void GeneratePages(int mod)
        {
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
