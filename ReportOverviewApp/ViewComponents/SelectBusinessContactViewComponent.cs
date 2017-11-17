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
    public class SelectBusinessContactViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SelectBusinessContactViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return View(null);
            }
            BusinessContact businessContact = await _context.BusinessContacts.Where(bc => bc.Name == name)?.SingleOrDefaultAsync();
            if(businessContact == null || businessContact.Name == null)
            {
                return View(null);
            }
            return View(businessContact);
        }
    }
}
