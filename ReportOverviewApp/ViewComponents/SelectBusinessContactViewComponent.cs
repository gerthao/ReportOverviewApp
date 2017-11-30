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
        public async Task<IViewComponentResult> InvokeAsync(int? reportId, int? businessContactId, bool remove=false)
        {
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Options = new DropdownOptions()
                {
                    BusinessContacts = await _context.BusinessContacts.OrderBy(bc => bc.Name).ToListAsync(),
                    Frequencies = await _context.Reports.Select(r => r.Frequency).Distinct().OrderBy(f => f).ToListAsync(),
                    Plans = await _context.Plans.Include(p => p.State).OrderBy(p => p.Name).ToListAsync(),
                    SourceDepartments = await _context.Reports.Select(r => r.SourceDepartment).Distinct().OrderBy(sd => sd).ToListAsync(),
                    States = await _context.States.OrderBy(s => s.PostalAbbreviation).ToListAsync(),
                    BusinessOwners = null
                }
            };
            if (reportId == null)
            {
                reportViewModel.Report = new Report();
                return View(reportViewModel);
            }
            Report report = await _context.Reports/*.Include(r => r.Deadlines)*/
                                               .Include(r => r.BusinessContact)
                                               //.Include(r => r.ReportPlanMapping)
                                               //     .ThenInclude(rpm => rpm.Plan)
                                               //     .ThenInclude(p => p.State)
                                               .Where(r => r.Id == reportId).SingleOrDefaultAsync();
            if (report == null)
            {
                reportViewModel.Report = new Report();
                return View(reportViewModel);
            } else
            {
                reportViewModel.Report = report;
            }
            
            if (remove)
            {
                reportViewModel.Report.BusinessContactId = null;
                reportViewModel.Report.BusinessContact = null;
                return View(reportViewModel);
            }
            if (businessContactId is null || !businessContactId.HasValue)
            {
                return View(reportViewModel);
            }
            BusinessContact businessContact = await _context.BusinessContacts.FindAsync(businessContactId.Value);
            if(businessContact == null || businessContact.Name == null)
            {
                return View(reportViewModel);
            }
            reportViewModel.Report.BusinessContact = businessContact;
            reportViewModel.Report.BusinessContactId = businessContact.Id;
            return View(reportViewModel);
        }
    }
}
