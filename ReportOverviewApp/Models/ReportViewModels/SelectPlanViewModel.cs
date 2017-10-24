using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class SelectPlanViewModel
    {
        public IEnumerable<Plan> Plans { get; set; }
        public IEnumerable<State> States { get; set; }
        public string State { get; set; }
        public SelectPlanViewModel(IEnumerable<Plan> plans, IEnumerable<State> states, string state = null)
        {
            State = state;
            States = states;
            if (String.IsNullOrEmpty(State))
            {
                Plans = plans.Where(p => p != null);
            } else
            {
                Plans = plans
                    .Where(p => p != null && !String.IsNullOrEmpty(p.Name))
                    .Where(p => !String.IsNullOrEmpty(p.State.Name))
                    .Where(p => p.State.PostalAbbreviation == State);
            }
        }
    }
}
