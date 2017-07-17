using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using Microsoft.AspNetCore.Authorization;
using ReportOverviewApp.Models.ReportViewModels;
using System.Reflection;

namespace ReportOverviewApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        ///  This method creates a ViewModel to displays records from the Report class.
        /// </summary>
        /// <param name="search">
        ///  This parameter determines the list of records retrieved containing the search.
        ///  parameter's values
        /// </param>
        /// <param name="column">
        ///  This parameter determines the sort order of the list of records by a certain Report field.
        /// </param>
        /// <param name="recordsPerPage">
        ///  This parameter determines the number of records displayed on a page.
        /// </param>
        /// <returns>
        ///  Returns a ReportViewModel which contains a IEnumberable<Report> based on the parameters given.
        ///  The returned object is used for the Index method.
        /// </returns>
        /// 
        //public static class ReportViewModelBuilder
        //{
        //    private static ReportViewModel viewModel;

        //}
        private ReportViewModel viewModel;

        private ReportViewModel GetReportViewModel(string search, string column, int recordsPerPage, int pageIndex, string plan)
        {
            if(viewModel == null) viewModel = new ReportViewModel()
            {
                Reports = from r in _context.Reports select r
            };
            if (!String.IsNullOrEmpty(plan))
            {
                viewModel.Reports = viewModel.Reports.Where(r => r.GroupName.Equals(plan));
                viewModel.Plan = plan;
            }
            if (!String.IsNullOrEmpty(search))
            {
                
                viewModel.Reports = viewModel.Reports.Where(r => r.Name.Contains(search));
                viewModel.Search = search;
            }
            if (!String.IsNullOrEmpty(column))
            {
                switch (column)
                {
                    case "ID":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.ID);
                        break;
                    case "Name":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.Name);
                        break;
                    case "Deadline":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.DateDue);
                        break;
                    default:
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.ID);
                        break;
                }
                viewModel.Column = column;
            }
            viewModel.GeneratePages(recordsPerPage);
            viewModel.Reports = viewModel.DisplayPage(pageIndex);
            return viewModel;
        }

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        [Authorize]
        public ActionResult Index(string search, string column, int entriesPerPage, int pageIndex, string plan)
        {
            return View(GetReportViewModel(search, column, entriesPerPage, pageIndex, plan));
        }

        // GET: Reports/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .SingleOrDefaultAsync(m => m.ID == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        public ActionResult SelectPlan(string state, string plan)
        {
            return View(GetReportViewModel(state, plan));
        }

        private ReportViewModel GetReportViewModel(string state, string plan)
        {
            if(viewModel == null) viewModel = new ReportViewModel()
            {
                Reports = from r in _context.Reports select r
            };
            if (!String.IsNullOrEmpty(state))
            {
                viewModel.State = state;
                viewModel.Reports = viewModel.Reports.Where(r => r.State.Equals(state));
            }
            if (!String.IsNullOrEmpty(plan))
            {
                viewModel.Plan = plan;
                viewModel.Reports = viewModel.Reports.Where(r => r.GroupName.Equals(plan));
            }
            viewModel.SetStates();
            viewModel.SetPlans(state);
            return viewModel;
        }

        // GET: Reports/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Name,Frequency,Done,DateDone,DateClientNotified,DateSent,ClientNotified,Sent,DateDue")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Done,DateDone,DateClientNotified,DateSent,ClientNotified,Sent,DateDue")] Report report)
        {
            if (id != report.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .SingleOrDefaultAsync(m => m.ID == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ID == id);
        }
    }
}
