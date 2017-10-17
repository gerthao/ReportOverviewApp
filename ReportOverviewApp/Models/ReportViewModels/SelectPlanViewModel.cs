﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class SelectPlanViewModel
    {
        public IEnumerable<Tuple<string, string>> StatePlanSelection { get; set; }
        public string State { get; set; }
        public SelectPlanViewModel(IEnumerable<Report> reports, string state = null)
        {
            State = state;
            if (String.IsNullOrEmpty(State))
            {
                StatePlanSelection = reports
                    .Where(r => !String.IsNullOrEmpty(r.State) && !String.IsNullOrEmpty(r.GroupName))
                    .Select(r => new { r.State, r.GroupName })
                    .AsEnumerable()
                    .Select(t => new Tuple<string, string>(t.State, t.GroupName));
            } else
            {
                StatePlanSelection = reports
                    .Where(r => !String.IsNullOrEmpty(r.State) && !String.IsNullOrEmpty(r.GroupName))
                    .Where(r => r.State == State)
                    .Select(r => new { r.State, r.GroupName })
                    .AsEnumerable()
                    .Select(t => new Tuple<string, string>(t.State, t.GroupName));
            }
        }
    }
}
