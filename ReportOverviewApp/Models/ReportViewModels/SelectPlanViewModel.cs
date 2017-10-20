using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportOverviewApp.Models.ReportViewModels
{
    public class SelectPlanViewModel
    {
        public IEnumerable<Plan> StatePlanSelection { get; set; }
        public string State { get; set; }
        public SelectPlanViewModel(IEnumerable<Plan> plans, string state = null)
        {
            State = state;
            if (String.IsNullOrEmpty(State))
            {
                StatePlanSelection = plans.Where(p => p != null);
            } else
            {
                StatePlanSelection = plans
                    .Where(p => p != null && !String.IsNullOrEmpty(p.Name))
                    .Where(p => !String.IsNullOrEmpty(p.State.Name))
                    .Where(p => p.State.Name == State);
            }
        }
    }
}
