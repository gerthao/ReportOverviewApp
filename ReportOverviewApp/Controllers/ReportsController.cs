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
using Newtonsoft.Json;
using System.Reflection;
using ReportOverviewApp.Helpers;

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
        private SearchTokenizer tokenizer;
        private ReportViewModel GetReportViewModel(string search, string column, int recordsPerPage, int pageIndex, string plan, DateTime? begin, DateTime? end)
        {
            if (viewModel == null)
            {
                viewModel = new ReportViewModel()
                {
                    Reports = from r in _context.Reports select r,
                    SortAscending = new Dictionary<string, bool>()
                    {
                        { "ID", true },
                        { "Name", true },
                        { "Deadline", true }
                    }
                };
            }
            HandlePlan(plan);
            HandleSearch(search);
            HandleSort(column);
            HandleDates(begin, end);
            viewModel.GeneratePages(recordsPerPage);
            viewModel.Reports = viewModel.DisplayPage(pageIndex);
            return viewModel;
        }
        
        public JsonResult JsonInfo(int? id)
        {
            var report = from r in _context.Reports where r.ID == id select r;
            return Json(report);
        }
        private void HandleSort(string column)
        {
            if (!String.IsNullOrEmpty(column)){
                viewModel.Column = column;
                switch (viewModel.Column){
                    case "ID":
                        viewModel.Reports = viewModel.SortAscending["ID"]? viewModel.Reports.OrderBy(report => report.ID) : viewModel.Reports.OrderByDescending(report => report.ID);
                        break;
                    case "Name":
                        viewModel.Reports = viewModel.SortAscending["Name"]? viewModel.Reports.OrderBy(report => report.Name) : viewModel.Reports.OrderByDescending(report => report.Name);
                        break;
                    case "Deadline":
                        viewModel.Reports = viewModel.SortAscending["Deadline"]? viewModel.Reports.OrderBy(report => report.Deadline()) : viewModel.Reports.OrderByDescending(report => report.Deadline());
                        break;
                    default:
                        viewModel.Reports = viewModel.SortAscending["ID"]? viewModel.Reports.OrderBy(report => report.ID) : viewModel.Reports.OrderByDescending(report => report.ID);
                        break;
                }
                
            }
        }

        private void HandleDates(DateTime? begin, DateTime? end)
        {
            if (begin != null){
                viewModel.Begin = begin;
                viewModel.Reports = viewModel.Reports.Where(r => r.Deadline() >= viewModel.Begin);
            }
            if (end != null){
                viewModel.End = end;
                viewModel.Reports = viewModel.Reports.Where(r => r.Deadline() <= viewModel.End);
            }
        }
        private void HandlePlan(string plan)
        {
            if (!String.IsNullOrEmpty(plan)){
                viewModel.Plan = plan;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.GroupName != null && r.GroupName.Equals(viewModel.Plan));
            }

        }
        private void HandleSearch(string search)
        {
            if (!String.IsNullOrEmpty(search)){
                viewModel.Search = search;
                //tokenizer.Tokenize(search);
                //Dictionary<string, SearchTokenizer.Mode> tokens = tokenizer.GetSearchTokens();
                //KeyValuePair<string, SearchTokenizer.Mode> kv;
                
                //for(int i = 0; i < tokens.Count(); i++)
                //{
                //    kv = tokens.ElementAt(i);
                //    switch (kv.Value)
                //    {

                //    }
                //}
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.Name != null && r.Name.ToLowerInvariant().Contains(viewModel.Search.ToLowerInvariant()));
            }
        }
        public ReportsController(ApplicationDbContext context) => _context = context;

        // GET: Reports
        [Authorize]
        public ActionResult Index(string search, string column, int entriesPerPage, int pageIndex, string plan, DateTime? begin = null, DateTime? end = null)
            => View(GetReportViewModel(search, column, entriesPerPage, pageIndex, plan, begin, end));

        // GET: Reports/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            if (report == null) return NotFound();
            return View(report);
        }

        public ActionResult SelectPlan(string state, string plan)
            => View(GetReportViewModelFromSelectPlan(state, plan));

        private ReportViewModel GetReportViewModelFromSelectPlan(string state, string plan)
        {
            if (viewModel == null) viewModel = new ReportViewModel(){Reports = from r in _context.Reports select r};
            if (!String.IsNullOrEmpty(state)){
                viewModel.State = state;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.State != null && r.State.Equals(state));
            }
            if (!String.IsNullOrEmpty(plan)){
                viewModel.Plan = plan;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.GroupName != null &&  r.GroupName.Equals(plan));
            }
            viewModel.SetStates();
            viewModel.SetPlans(state);
            return viewModel;
        }

        // GET: Reports/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Create([Bind("ID,Name,Done,ClientNotified,Sent,DateDue,DateDone,DateClientNotified,DateSent,BusinessContact,BusinessOwner,DueDate1,DueDate2,DueDate3,DueDate4,Frequency,DayDue,DeliveryFunction,WorkInstructions,Notes,DaysAfterQuarter,FolderLocation,ReportType,RunWith,DeliveryMethod,DeliveryTo,EffectiveDate,TerminationDate,GroupName,State,ReportPath,OtherDepartment,SourceDepartment,QualityIndicator,ERSReportLocation,ERRStatus,DateAdded,SystemRefreshDate,LegacyReportID,LegacyReportIDR2,ERSReportName,OtherReportLocation,OtherReportName")] Report report)
        {
            if (ModelState.IsValid){
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
            if (id == null) return NotFound();
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            if (report == null) return NotFound();
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Done,ClientNotified,Sent,DateDue,DateDone,DateClientNotified,DateSent,BusinessContact,BusinessOwner,DueDate1,DueDate2,DueDate3,DueDate4,Frequency,DayDue,DeliveryFunction,WorkInstructions,Notes,DaysAfterQuarter,FolderLocation,ReportType,RunWith,DeliveryMethod,DeliveryTo,EffectiveDate,TerminationDate,GroupName,State,ReportPath,OtherDepartment,SourceDepartment,QualityIndicator,ERSReportLocation,ERRStatus,DateAdded,SystemRefreshDate,LegacyReportID,LegacyReportIDR2,ERSReportName,OtherReportLocation,OtherReportName")] Report report)
        {
            if (id != report.ID) return NotFound();
            if (ModelState.IsValid){
                try{
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!ReportExists(report.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            if (report == null) return NotFound();
            return View(report);
        }

        // POST: Reports/Delete/5
        [Authorize, HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReportExists(int id) => _context.Reports.Any(e => e.ID == id);
    }
}
