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
using System.Globalization;
using NToastNotify;

namespace ReportOverviewApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IToastNotification _toastNotification;
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

        private async Task<ReportViewModel> GetReportViewModelAsync(string search, string column, int recordsPerPage, int pageIndex, string state, string plan, string begin, string end, string frequency, string businessContact, string businessOwner, string sourceDepartment)
        {
            if (viewModel == null)
            {
                viewModel = new ReportViewModel()
                {
                    Reports = await _context.Reports.ToListAsync()
                };
                viewModel.States = viewModel.Reports.Select(r => r.State).Distinct().OrderBy(s => s);
                viewModel.Plans = viewModel.Reports.Select(r => r.GroupName).Distinct().OrderBy(p => p);
                viewModel.Frequencies = viewModel.Reports.Select(r => r.Frequency).Distinct().OrderBy(f => f);
                viewModel.BusinessContacts = viewModel.Reports.Select(r => r.BusinessContact).Distinct().OrderBy(bc => bc);
                viewModel.BusinessOwners = viewModel.Reports.Select(r => r.BusinessOwner).Distinct().OrderBy(bo => bo);
                viewModel.SourceDepartments = viewModel.Reports.Select(r => r.SourceDepartment).Distinct().OrderBy(sd => sd);
            }
            HandleStateAndPlan(state, plan);
            HandleSearch(search);
            HandleBusinessContact(businessContact);
            HandleBusinessOwner(businessOwner);
            HandleSourceDepartment(sourceDepartment);
            HandleSort(column);
            HandleDates(begin, end);
            HandleFrequency(frequency);
            viewModel.GeneratePages(recordsPerPage);
            viewModel.Reports = viewModel.DisplayPage(pageIndex);
            return viewModel;
        }

        private void HandleBusinessContact(string businessContact)
        {
            if (!String.IsNullOrEmpty(businessContact))
            {
                viewModel.BusinessContact = businessContact;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.BusinessContact == businessContact);
            }
        }
        private void HandleBusinessOwner(string businessOwner)
        {
            if (!String.IsNullOrEmpty(businessOwner))
            {
                viewModel.BusinessOwner = businessOwner;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.BusinessOwner == businessOwner);
            }
        }
        private void HandleSourceDepartment(string sourceDepartment)
        {
            if (!String.IsNullOrEmpty(sourceDepartment))
            {
                viewModel.SourceDepartment = sourceDepartment;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.SourceDepartment == sourceDepartment);
            }
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
            if (!String.IsNullOrEmpty(column)) {
                viewModel.Column = column;
                switch (viewModel.Column) {
                    case "ID":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.ID);
                        break;
                    case "Name":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.Name);
                        break;
                    case "Deadline":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.CurrentDeadline());
                        break;
                    case "Finished On":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.DateDone);
                        break;
                    case "Notified On":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.DateClientNotified);
                        break;
                    case "Sent On":
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.DateSent);
                        break;
                    default:
                        viewModel.Reports = viewModel.Reports.OrderBy(report => report.ID);
                        break;
                }

            }
        }
        private void HandleDates(string begin, string end)
        {
            IEnumerable<(Report report, DateTime? deadline)> list = viewModel.Reports.Select(r => (r, r.CurrentDeadline()));
            list = list.Where(r => r.deadline.HasValue);
            if (begin != null) {
                DateTime beginDate;
                if (DateTime.TryParse(begin, out beginDate)) {
                    viewModel.Begin = beginDate;
                    list = list.Where(r => r.deadline.Value >= beginDate);
                }
            }
            if (end != null) {
                DateTime endDate;
                if (DateTime.TryParse(end, out endDate))
                {
                    viewModel.End = endDate;
                    list = list.Where(r => r.deadline.Value <= endDate);
                }
            }
            viewModel.Reports = list.Select(r => r.report);
        }
        private void HandleStateAndPlan(string state, string plan)
        {
            if (!String.IsNullOrEmpty(state))
            {
                viewModel.State = state;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.State != null && r.State.Equals(viewModel.State));
            }
            if (!String.IsNullOrEmpty(plan)) {
                viewModel.Plan = plan;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.GroupName != null && r.GroupName.Equals(viewModel.Plan));
            }
        }
        private void HandleSearch(string search)
        {
            if (!String.IsNullOrEmpty(search)) {
                viewModel.Search = search;
                viewModel.Reports = viewModel.Reports.Where(r => r != null && r.Name != null && r.Name.ToLowerInvariant().Contains(viewModel.Search.ToLowerInvariant()));
            }
        }
        public ReportsController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            userLogFactory = new UserLogFactory();
        }

        private async Task<SelectPlanViewModel> GetSelectPlanViewModelAsync(string state)
        {
            return new SelectPlanViewModel(await _context.Reports.ToListAsync(), state);
        }

        // GET: Reports
        [Authorize]
        public async Task<IActionResult> Index(string search, string column, int entriesPerPage, int pageIndex, string state, string plan, string frequency, string businessContact, string businessOwner, string sourceDepartment, string begin = null, string end = null)
        {
            return View(await GetReportViewModelAsync(search, column, entriesPerPage, pageIndex, state, plan, begin, end, frequency, businessContact, businessOwner, sourceDepartment));
        }
        // GET: Reports/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            if (report == null) return NotFound();
            return View(report);
        }

        public async Task<IActionResult> SelectPlan(string state)
            => View(await GetSelectPlanViewModelAsync(state));

        // GET: Reports/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Create([Bind("ID,Name,Done,ClientNotified,Sent,DateDue,DateDone,DateClientNotified,DateSent,BusinessContact,BusinessOwner,DueDate1,DueDate2,DueDate3,DueDate4,Frequency,DayDue,DeliveryFunction,WorkInstructions,Notes,DaysAfterQuarter,FolderLocation,ReportType,RunWith,DeliveryMethod,DeliveryTo,EffectiveDate,TerminationDate,GroupName,State,ReportPath,OtherDepartment,SourceDepartment,QualityIndicator,ERSReportLocation,ERRStatus,DateAdded,SystemRefreshDate,LegacyReportID,LegacyReportIDR2,ERSReportName,OtherReportLocation,OtherReportName")] Report report)
        {
            ToastMessage toast = null;
            if (ModelState.IsValid){
                _context.Add(report);
                _context.Add(userLogFactory.Build(GetCurrentUserID(), report.ID, $"\"{report.Name}\" has been created."));
                toast = new ToastMessage(message: $"{report.Name} has been successfully created.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                await _context.SaveChangesAsync();
                ShowToast(toast);
                return RedirectToAction("Index");
            }
            toast = new ToastMessage(message: $"One or more fields in the form are not valid.", title: "Changes Needed", toasType: ToastEnums.ToastType.Warning, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
            ShowToast(toast);
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
            ToastMessage toast = null;
            if (id != report.ID)
            {
                toast = new ToastMessage(message: $"IDs do not match", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                ShowToast(toast);
                return NotFound();
            }
            if (ModelState.IsValid){
                try{
                    _context.Add(userLogFactory.Build(GetCurrentUserID(), report.ID, $"\"{report.Name}\" has been edited."));
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                    toast = new ToastMessage(message: $"{ report.Name} has been successfully edited.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                }
                catch (DbUpdateConcurrencyException){
                    if (!ReportExists(report.ID))
                    {
                        toast = new ToastMessage(message: $"{report.Name} was not found in the database.", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                        ShowToast(toast);
                        return NotFound();
                    }
                    else throw;
                }
                ShowToast(toast);
                return RedirectToAction("Index");
            }
            toast = new ToastMessage(message: $"One or more fields in the form are not valid.", title: "Changes Needed", toasType: ToastEnums.ToastType.Warning, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
            ShowToast(toast);
            return View(report);
        }
        
        private void ShowToast(ToastMessage toastMessage)
        {
            _toastNotification.AddToastMessage(toastMessage.Title, toastMessage.Message, toastMessage.ToastType, toastMessage.ToastOptions);
        }

        //private string CompareChanges(ref Report old, ref Report updated)
        //{
        //    if (old == null)
        //    {
        //        old = new Report();
        //    }
        //    if (updated == null)
        //    {
        //        updated = new Report();
        //    }
        //    if (old.Equals(updated)) return "No Apparent Changes Made.";
        //    StringBuilder messageBuilder = new StringBuilder();
        //    messageBuilder
        //        .Append(Compare("Name", old.Name, updated.Name))
        //        .Append(Compare("Report Finished", old.Done, updated.Done))
        //        .Append(Compare("Report Client Notified", old.ClientNotified, updated.ClientNotified))
        //        .Append(Compare("Report Sent", old.Sent, updated.Sent))
        //        .Append(Compare("Date Finished", old.DateDone, updated.DateDone))
        //        .Append(Compare("Date Notified", old.DateClientNotified, updated.DateClientNotified))
        //        .Append(Compare("Date Sent", old.DateSent, updated.DateSent))
        //        .Append(Compare("Business Contact", old.BusinessContact, updated.BusinessContact))
        //        .Append(Compare("Business Owner", old.BusinessContact, updated.BusinessContact))
        //        .Append(Compare("Due Date 1", old.DueDate1, updated.DueDate1))
        //        .Append(Compare("Due Date 2", old.DueDate2, updated.DueDate2))
        //        .Append(Compare("Due Date 3", old.DueDate3, updated.DueDate3))
        //        .Append(Compare("Due Date 4", old.DueDate4, updated.DueDate4))
        //        .Append(Compare("Frequency", old.Frequency, updated.Frequency))
        //        .Append(Compare("Day Due", old.DayDue, updated.DayDue))
        //        .Append(Compare("Delivery Function", old.DeliveryFunction, updated.DeliveryFunction))
        //        .Append(Compare("Work Instructions", old.WorkInstructions, updated.WorkInstructions))
        //        .Append(Compare("Notes", old.Notes, updated.Notes))
        //        .Append(Compare("Days After Quarter", old.DaysAfterQuarter, updated.DaysAfterQuarter))
        //        .Append(Compare("Folder Location", old.FolderLocation, updated.FolderLocation))
        //        .Append(Compare("Report Type", old.ReportType, updated.ReportType))
        //        .Append(Compare("Run With", old.RunWith, updated.RunWith))
        //        .Append(Compare("Delivery Method", old.DeliveryMethod, updated.DeliveryMethod))
        //        .Append(Compare("Delivery To", old.DeliverTo, updated.DeliverTo))
        //        .Append(Compare("Effective Date", old.EffectiveDate, updated.EffectiveDate))
        //        .Append(Compare("Termination Date", old.TerminationDate, updated.TerminationDate))
        //        .Append(Compare("Plan", old.GroupName, updated.GroupName))
        //        .Append(Compare("State", old.State, updated.State))
        //        .Append(Compare("Report Path", old.ReportPath, updated.ReportPath))
        //        .Append(Compare("Other Department", old.OtherDepartment, updated.OtherDepartment))
        //        .Append(Compare("Source Department", old.SourceDepartment, updated.SourceDepartment))
        //        .Append(Compare("Quality Indicator", old.QualityIndicator, updated.QualityIndicator))
        //        .Append(Compare("ERS Report Location", old.ERSReportLocation, updated.ERSReportLocation))
        //        .Append(Compare("ERR Status", old.ERRStatus, updated.ERRStatus))
        //        .Append(Compare("Date Added", old.DateAdded, updated.DateAdded))
        //        .Append(Compare("System Refresh Date", old.SystemRefreshDate, updated.SystemRefreshDate))
        //        .Append(Compare("Legacy Report ID", old.LegacyReportID, updated.LegacyReportID))
        //        .Append(Compare("Legacy Report ID R2", old.LegacyReportIDR2, updated.LegacyReportIDR2))
        //        .Append(Compare("ERS Report Name", old.ERSReportName, updated.ERSReportName))
        //        .Append(Compare("Other Report Location", old.OtherReportLocation, updated.OtherReportLocation))
        //        .Append(Compare("Other Report Name", old.OtherReportName, updated.OtherReportName));
        //    string Compare<T>(string header, T item1, T item2)
        //    {
        //        string item1Entry = item1 != null ? item1.ToString() : "null";
        //        string item2Entry = item2 != null ? item2.ToString() : "null"; ;
        //        return (!item1Entry.Equals(item2Entry)) ? ($"{header}: {item1Entry} => {item2Entry}\n") : (String.Empty);
        //    }
        //    return messageBuilder.ToString();
        //}

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
            ToastMessage toast = null;
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.ID == id);
            _context.Reports.Remove(report);
            _context.Add(userLogFactory.Build(GetCurrentUserID(), report.ID, $"\"{report.Name}\" has been deleted."));
            toast = toast = new ToastMessage(message: $"{report.Name} has been successfully deleted.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight});
            await _context.SaveChangesAsync();
            ShowToast(toast);
            return RedirectToAction("Index");
        }
        private bool ReportExists(int id) => _context.Reports.Any(e => e.ID == id);
    }
}
