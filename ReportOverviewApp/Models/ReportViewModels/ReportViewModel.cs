﻿using System;
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
        private int Pages { get; set; }
        private int PageSize { get; set; }
        public int CurrentPage { get; private set; } = 1;
        public const int DEFAULT_PAGE_SIZE = 20;


        public string Search { get; set; }
        public string Column { get; set; }
        public string State { get; set; }
        public string Plan { get; set; }

        public IEnumerable<string> Plans { get; private set; }
        public IEnumerable<string> States { get; private set; }

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
            if (mod <= 0) {
                mod = DEFAULT_PAGE_SIZE;
            }
            PageSize = mod;
            Pages = Reports.Count() / PageSize;
            if (Reports.Count() % PageSize > 0)
            {
                Pages++;
            }
        }

        public int PagesCount()
        {
            return Pages;
        }

        public void SetPlans(string chosenState)
        {
            if (String.IsNullOrEmpty(chosenState))
            {
                Plans = (from r in Reports select r.GroupName).Distinct().OrderBy(p => p.ToString());
            }
            else
            {
                Plans = (from r in Reports where r.State.Equals(chosenState) select r.GroupName).Distinct().OrderBy(p => p.ToString());
            }
        }
        public void SetStates()
        {
            States = (from r in Reports select r.State).Distinct().OrderBy(p => p.ToString());
        }

        public IEnumerable<Report> DisplayPage(int index)
        {
            if(index <= 0)
            {
                index = 1;
            }
            CurrentPage = index;

            return Reports.Skip((CurrentPage - 1)*PageSize).Take(PageSize);
        }
        //public string DisplayMessage(DateTime? date)
        //{
        //    return String.Format("{0:MM/dd}", date);
        //}
    }
}
