using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ReportOverviewApp.Models.WidgetModels;

namespace ReportOverviewApp.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Report> Reports { get;}
        public IEnumerable<Widget> Widgets { get;}
        public IEnumerable<ApplicationUser> Users { get;}
    }
}
