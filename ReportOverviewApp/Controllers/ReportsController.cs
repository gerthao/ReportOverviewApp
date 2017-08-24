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
using System.Text;

namespace ReportOverviewApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ReportViewModel viewModel;
        private UserLogFactory userLogFactory;

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

        private async Task<ReportViewModel> GetReportViewModelAsync(string search, string column, int recordsPerPage, int pageIndex, string state, string plan, DateTime? begin, DateTime? end, string frequency)
        {
            if (viewModel == null)
            {
                viewModel = new ReportViewModel()
                {
                    Reports = await _context.Reports.ToListAsync(),
                    SortAscending = new Dictionary<string, bool>()
                    {
                        { "ID", true },
                        { "Name", true },
                        { "Deadline", true }
                    }
                };
                viewModel.States = viewModel.Reports.Select(r => r.State).OrderBy(s => s).Distinct();
                viewModel.Plans = viewModel.Reports.Select(r => r.GroupName).OrderBy(p => p).Distinct();
            }
            HandleStateAndPlan(state, plan);
            HandleSearch(search);
            HandleSort(column);
            HandleDates(begin, end);
            HandleFrequency(frequency);
            viewModel.GeneratePages(recordsPerPage);
            viewModel.Reports = viewModel.DisplayPage(pageIndex);
            return viewModel;
        }
        
       
        private void HandleFrequency(string frequency)
        {
            if (!String.IsNullOrEmpty(frequency))
            {
                viewModel.Frequency = frequency;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.Frequency != null && r.Frequency.Equals(frequency));
            }
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
                        viewModel.Reports = viewModel.SortAscending["Deadline"]? viewModel.Reports.OrderBy(report => report.CurrentDeadline()) : viewModel.Reports.OrderByDescending(report => report.CurrentDeadline());
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
                viewModel.Reports = viewModel.Reports.Where(r => r.CurrentDeadline() >= viewModel.Begin);
            }
            if (end != null){
                viewModel.End = end;
                viewModel.Reports = viewModel.Reports.Where(r => r.CurrentDeadline() <= viewModel.End);
            }
        }
        private void HandleStateAndPlan(string state, string plan)
        {
            if (!String.IsNullOrEmpty(state))
            {
                viewModel.State = state;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.State != null && r.State.Equals(viewModel.State));
            }
            if (!String.IsNullOrEmpty(plan)){
                viewModel.Plan = plan;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.GroupName != null && r.GroupName.Equals(viewModel.Plan));
            }

        }
        private void HandleSearch(string search)
        {
            if (!String.IsNullOrEmpty(search)){
                viewModel.Search = search;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.Name != null && r.Name.ToLowerInvariant().Contains(viewModel.Search.ToLowerInvariant()));
            }
        }
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
            userLogFactory = new UserLogFactory();
        }

        // GET: Reports
        [Authorize]
        public async Task<IActionResult> Index(string search, string column, int entriesPerPage, int pageIndex, string state, string plan, string frequency, DateTime? begin = null, DateTime? end = null)
            => View(await GetReportViewModelAsync(search, column, entriesPerPage, pageIndex, state, plan, begin, end, frequency));

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

        //public ActionResult Directory(string state, string plan)
        //    => View(GetReportViewModelFromSelectPlan(state, plan));

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
                _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{report.Name}\" has been created."));
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

        private string GetCurrentUserID() => _context.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault().UserName;

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Done,ClientNotified,Sent,DateDue,DateDone,DateClientNotified,DateSent,BusinessContact,BusinessOwner,DueDate1,DueDate2,DueDate3,DueDate4,Frequency,DayDue,DeliveryFunction,WorkInstructions,Notes,DaysAfterQuarter,FolderLocation,ReportType,RunWith,DeliveryMethod,DeliveryTo,EffectiveDate,TerminationDate,GroupName,State,ReportPath,OtherDepartment,SourceDepartment,QualityIndicator,ERSReportLocation,ERRStatus,DateAdded,SystemRefreshDate,LegacyReportID,LegacyReportIDR2,ERSReportName,OtherReportLocation,OtherReportName")] Report report)
        {
            if (id != report.ID) return NotFound();
            if (ModelState.IsValid){
                try{
                    //Report old = (_context.Reports.Where(r => r.ID == id).SingleOrDefault()).Copy();
                    //Report newer = report.Copy();
                    _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{report.Name}\" has been edited."/*, CompareChanges(null, newer)*/));
                    //await _context.SaveChangesAsync();

                    //old = null;
                    //newer = null;

                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!ReportExists(report.ID)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index", new {search = viewModel.Search, entriesPerPage = viewModel.PageSize, pageIndex = viewModel.CurrentPage, state = viewModel.State, plan = viewModel.Plan, begin = viewModel.Begin, end = viewModel.End, frequency = viewModel.Frequency});
            }
            return View(report);
        }

        private string CompareChanges(Report old, Report updated)
        {
            if(old == null)
            {
                old = new Report();
            }
            if(updated == null)
            {
                updated = new Report();
            }
            if (old.Equals(updated)) return "No Apparent Changes Made.";
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder
                .Append(Compare("ID", old.ID, updated.ID))
                .Append(Compare("Name", old.Name, updated.Name))
                .Append(Compare("Report Finished", old.Done, updated.Done))
                .Append(Compare("Report Client Notified", old.ClientNotified, updated.ClientNotified))
                .Append(Compare("Report Sent", old.Sent, updated.Sent))
                .Append(Compare("Date Finished", old.DateDone, updated.DateDone))
                .Append(Compare("Date Notified", old.DateClientNotified, updated.DateClientNotified))
                .Append(Compare("Date Sent", old.DateSent, updated.DateSent))
                .Append(Compare("Business Contact", old.BusinessContact, updated.BusinessContact))
                .Append(Compare("Business Owner", old.BusinessContact, updated.BusinessContact))
                .Append(Compare("Due Date 1", old.DueDate1, updated.DueDate1))
                .Append(Compare("Due Date 2", old.DueDate2, updated.DueDate2))
                .Append(Compare("Due Date 3", old.DueDate3, updated.DueDate3))
                .Append(Compare("Due Date 4", old.DueDate4, updated.DueDate4))
                .Append(Compare("Frequency", old.Frequency, updated.Frequency))
                .Append(Compare("Day Due", old.DayDue, updated.DayDue))
                .Append(Compare("Delivery Function", old.DeliveryFunction, updated.DeliveryFunction))
                .Append(Compare("Work Instructions", old.WorkInstructions, updated.WorkInstructions))
                .Append(Compare("Notes", old.Notes, updated.Notes))
                .Append(Compare("Days After Quarter", old.DaysAfterQuarter, updated.DaysAfterQuarter))
                .Append(Compare("Folder Location", old.FolderLocation, updated.FolderLocation))
                .Append(Compare("Report Type", old.ReportType, updated.ReportType))
                .Append(Compare("Run With", old.RunWith, updated.RunWith))
                .Append(Compare("Delivery Method", old.DeliveryMethod, updated.DeliveryMethod))
                .Append(Compare("Delivery To", old.DeliverTo, updated.DeliverTo))
                .Append(Compare("Effective Date", old.EffectiveDate, updated.EffectiveDate))
                .Append(Compare("Termination Date", old.TerminationDate, updated.TerminationDate))
                .Append(Compare("Plan", old.GroupName, updated.GroupName))
                .Append(Compare("State", old.State, updated.State))
                .Append(Compare("Report Path", old.ReportPath, updated.ReportPath))
                .Append(Compare("Other Department", old.OtherDepartment, updated.OtherDepartment))
                .Append(Compare("Source Department", old.SourceDepartment, updated.SourceDepartment))
                .Append(Compare("Quality Indicator", old.QualityIndicator, updated.QualityIndicator))
                .Append(Compare("ERS Report Location", old.ERSReportLocation, updated.ERSReportLocation))
                .Append(Compare("ERR Status", old.ERRStatus, updated.ERRStatus))
                .Append(Compare("Date Added", old.DateAdded, updated.DateAdded))
                .Append(Compare("System Refresh Date", old.SystemRefreshDate, updated.SystemRefreshDate))
                .Append(Compare("Legacy Report ID", old.LegacyReportID, updated.LegacyReportID))
                .Append(Compare("Legacy Report ID R2", old.LegacyReportIDR2, updated.LegacyReportIDR2))
                .Append(Compare("ERS Report Name", old.ERSReportName, updated.ERSReportName))
                .Append(Compare("Other Report Location", old.OtherReportLocation, updated.OtherReportLocation))
                .Append(Compare("Other Report Name", old.OtherReportName, updated.OtherReportName));

            string Compare<T>(string header, T item1, T item2)
            {
                string item1Entry = item1 != null ? item1.ToString() : "null";
                string item2Entry = item2 != null ? item2.ToString() : "null"; ;
                return (!item1Entry.Equals(item2Entry)) ? ($"{header}: {item1Entry} => {item2Entry}\n") : (String.Empty);
            }
               
            
            return messageBuilder.ToString();
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
            _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{report.Name}\" has been deleted."));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ReportExists(int id) => _context.Reports.Any(e => e.ID == id);
    }
}
