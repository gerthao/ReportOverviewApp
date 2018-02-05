using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.ReportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ReportOverviewApp.ViewComponents
{
    public class ExportViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ExportViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            if(name.ToLower() == "plans")
            {
                return View("Plans");
            }
            return View();
        }
    }
}
