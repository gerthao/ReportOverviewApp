using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using Microsoft.AspNetCore.Authorization;
using ReportOverviewApp.Models.ReportViewModels;
using ReportOverviewApp.Helpers;
using System.Text;
using NToastNotify;

namespace ReportOverviewApp.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IToastNotification _toastNotification;
        private ReportListViewModel viewModel;
        private UserLogFactory userLogFactory;

        /// <summary>
        ///  This method creates a ViewModel to displays records from the Report class.
        /// </summary>
        /// <param name="search">
        ///  Determines the list of records retrieved containing the search.
        ///  parameter's values
        /// </param>
        /// <param name="column">
        ///  Determines the sort order of the list of records by a certain Report field.
        /// </param>
        /// <param name="recordsPerPage">
        ///  Determines the number of records displayed on a page.
        /// </param>
        /// <returns>
        ///  Returns a ReportViewModel based on the parameters given.
        ///  The returned object is used for the Index method.
        /// </returns>
        /// 

        private async Task<ReportListViewModel> GetReportViewModelAsync(string search, string column, int recordsPerPage, int pageIndex, string state, string plan, string begin, string end, string frequency, string businessContact, string businessOwner, string sourceDepartment)
        {
            DropdownOptions options = new DropdownOptions()
            {
                States = await _context.States.OrderBy(s => s.PostalAbbreviation).ToListAsync(),
                Plans = await _context.Plans.OrderBy(p => p.Name).ToListAsync(),
                Frequencies = await _context.Reports.Select(r => r.Frequency).OrderBy(f => f).Distinct().ToListAsync(),
                BusinessContacts = await _context.BusinessContacts.OrderBy(bc => bc.Name).ToListAsync(),
                BusinessOwners = await _context.BusinessContacts.Select(bc => bc.BusinessOwner).OrderBy(bo => bo).Distinct().ToListAsync(),
                SourceDepartments = await _context.Reports.Select(r => r.SourceDepartment).OrderBy(sd => sd).Distinct().ToListAsync()
            };
            Filters filters = new Filters()
            {
                State = state,
                Plan = plan,
                Search = search,
                BusinessContact = businessContact,
                BusinessOwner = businessOwner,
                SourceDepartment = sourceDepartment,
                Column = column,
                BeginString = begin,
                EndString = end,
                Frequency = frequency
            };
            viewModel = new ReportListViewModel(options: options, filters: filters)
            {
                Reports = await _context.Reports.Include(r => r.Deadlines)
                                                .Include(r => r.BusinessContact)
                                                .Include(r => r.ReportPlanMapping)
                                                    .ThenInclude(rpm => rpm.Plan)
                                                        .ThenInclude(p => p.State).ToListAsync(),
                ReportDeadlines = await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd != null).ToListAsync()
            };
            viewModel.Options = options;
            viewModel.Filters = filters;
            viewModel.ApplyFilters();
            viewModel.GeneratePages(recordsPerPage);
            viewModel.Reports = viewModel.DisplayPage(pageIndex);
            return viewModel;
        }

        public IActionResult CreateReport()
        {
            return ViewComponent("CreateReport");
        }
        public IActionResult EditReport(int? id)
        {
            return ViewComponent("EditReport", new { reportId = id });
        }
        public IActionResult DeleteReport(int? id)
        {
            return ViewComponent("DeleteReport", new { reportId = id });
        }
        public IActionResult SelectBusinessContact(int? id, int? contactId, bool removal=false)
        {
            return ViewComponent("SelectBusinessContact", new { reportId = id, businessContactId = contactId, remove = removal });
        }
        public IActionResult Plans(int? id, string name, IEnumerable<int> planIds, bool isModified = false, bool removal = false)
        {
            return ViewComponent("Plans", new { reportId = id, planName = name, plans = planIds, changed = isModified, remove = removal });
        }
        public IActionResult EditReportDeadline(int? id)
        {
            return ViewComponent("EditReportDeadline", new { deadlineId = id } );
        }

        public IActionResult UpdateDeadlinesAsync()
        {
            if (_context != null && _context.ReportDeadlines != null)
            {
                //var mostRecent = await _context.Reports.Include(r => r.Deadlines).Select(r => r.Deadlines.Any()? r.Deadlines.OrderByDescending(rd => rd.Deadline).First() : null).ToListAsync();
                //var list = mostRecent.Where(rd => rd != null && rd.Report != null && rd.Deadline < rd.Report.CurrentDeadline()).Select(rd => new ReportDeadline() { Deadline = rd.Report.CurrentDeadline().Value, ReportId = rd.ReportId });
                //if (list.Count() > 0)
                //{
                //    await _context.ReportDeadlines.AddRangeAsync(list);
                //    await _context.SaveChangesAsync();
                //}
                foreach(Report r in _context.Reports.Include(r => r.Deadlines))
                {
                    DateTime? currentCalculatedDeadline = r.CurrentDeadline();
                    if(!(currentCalculatedDeadline == null || !currentCalculatedDeadline.HasValue))
                    {
                        if(r.Deadlines == null || r.Deadlines.Count() == 0)
                        {
                            ReportDeadline reportDeadline = new ReportDeadline()
                            {
                                ReportId = r.Id,
                                Deadline = currentCalculatedDeadline.Value
                            };
                            _context.ReportDeadlines.Add(reportDeadline);
                        } else
                        {
                            ReportDeadline mostRecentReportDeadline = r.Deadlines.OrderByDescending(rd => rd.Deadline).First();
                            if (currentCalculatedDeadline > mostRecentReportDeadline.Deadline)
                            {
                                ReportDeadline reportDeadline = new ReportDeadline()
                                {
                                    ReportId = r.Id,
                                    Deadline = currentCalculatedDeadline.Value
                                };
                                _context.ReportDeadlines.Add(reportDeadline);
                            }
                        }
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public ReportsController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            userLogFactory = new UserLogFactory();

        }

        private async Task<SelectPlanViewModel> GetSelectPlanViewModelAsync(string state)
        {
            return new SelectPlanViewModel(await _context.Plans.Include(p => p.State).ToListAsync(), await _context.States.OrderBy(s => s.Name).ToListAsync(), state);
        }

        // GET: Reports
        [Authorize]
        public async Task<IActionResult> Index(string search, string column, int entriesPerPage, int pageIndex, string state, string plan, string frequency, string businessContact, string businessOwner, string sourceDepartment, string begin = null, string end = null)
        {
            //await UpdateDeadlinesAsync();
            return View(await GetReportViewModelAsync(search, column, entriesPerPage, pageIndex, state, plan, begin, end, frequency, businessContact, businessOwner, sourceDepartment));
        }
        // GET: Reports/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.Include(r => r.Deadlines).SingleOrDefaultAsync(m => m.Id == id);
            if (report == null) return NotFound();
            return View(report);
        }

        public IActionResult GetSelectPlanList(string state = null)
        {
            return ViewComponent("SelectPlanList", new { State = state });
        }

        [Authorize]
        public async Task<IActionResult> SelectPlan(string state)
            => View(await GetSelectPlanViewModelAsync(state));

        // GET: Reports/Create
        [Authorize]
        public IActionResult Create() => View();
        
        public async Task<IActionResult> UpcomingReports()
        {
            return View(await _context.ReportDeadlines.Where(rd => rd != null && rd.Deadline != null && rd.Deadline >= DateTime.Today && rd.Deadline <= DateTime.Today.AddDays(7)).Include(rd => rd.Report).OrderBy(rd => rd.Deadline).ThenBy(rd => rd.Report.Name).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> PastReports()
        {
            return View(await _context.ReportDeadlines.Where(rd => rd != null && rd.Deadline != null && rd.Deadline < DateTime.Today).Include(rd => rd.Report).OrderByDescending(rd => rd.Deadline).ThenBy(rd => rd.Report.Name).ToListAsync());
        }

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Create([Bind("Report, Plans")] ReportViewModel reportViewModel)
        {
            ToastMessage toast = null;
            if (ModelState.IsValid){
                if(reportViewModel.Report.DateAdded == null)
                {
                    reportViewModel.Report.DateAdded = DateTime.Now;
                }
                reportViewModel.Report.BusinessContact = null;
                _context.Add(reportViewModel.Report);
                await _context.SaveChangesAsync();
                List<ReportPlanMap> list = reportViewModel.Plans is null || reportViewModel.Plans.Count() == 0 ? new List<ReportPlanMap>() : reportViewModel.Plans.Select(i => new ReportPlanMap() { PlanId = i, ReportId = reportViewModel.Report.Id }).ToList();
                reportViewModel.Report.ReportPlanMapping = list;
                _context.Update(reportViewModel.Report);
                _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{reportViewModel.Report.Name}\" has been created."));
                await _context.SaveChangesAsync();
                toast = new ToastMessage(message: $"{reportViewModel.Report.Name} has been successfully created.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                ShowToast(toast);
                return RedirectToAction("Index");
            }
            toast = new ToastMessage(message: $"One or more fields in the form are not valid.", title: "Changes Needed", toasType: ToastEnums.ToastType.Warning, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
            ShowToast(toast);
            return View(reportViewModel);
        }

        [Authorize]
        public async Task<IActionResult> EditDeadline(int? id)
        {
            if (id == null) return NotFound();
            var deadline = await _context.ReportDeadlines.Include(rd => rd.Report).SingleOrDefaultAsync(d => d.Id == id);
            if (deadline == null) return NotFound();
            return View(deadline);
        }
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> EditDeadline(int? id, [Bind("Id,Deadline,RunDate,ApprovalDate,SentDate,ReportId")] ReportDeadline reportDeadline)
        {
            ToastMessage toast = null;
            if(id != reportDeadline.Id)
            {
                toast = new ToastMessage(message: $"IDs do not match", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                ShowToast(toast);
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var unmodifiedDeadline = _context.ReportDeadlines.AsNoTracking().Include(rd => rd.Report).SingleOrDefault(d => d.Id == id);
                    _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"Status for {unmodifiedDeadline.Report.Name}\" has been updated.", CompareChanges(unmodifiedDeadline, reportDeadline)));
                    _context.Update(reportDeadline);
                    await _context.SaveChangesAsync();
                    toast = new ToastMessage(message: $"Deadline ({reportDeadline.Deadline.ToString("MM/dd/yyyy")}) for {unmodifiedDeadline.Report.Name} has been successfully edited.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportDeadlineExists(reportDeadline.Id))
                    {
                        toast = new ToastMessage(message: $"Deadline ({reportDeadline.Deadline.ToString("MM/dd/yyyy")}) was not found in the database.", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
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
            return View(reportDeadline);
        }
        
        

        // GET: Reports/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.Include(r => r.BusinessContact).Include(r => r.ReportPlanMapping).ThenInclude(rpm => rpm.Plan).ThenInclude(p => p.State).SingleOrDefaultAsync(m => m.Id == id);
            if (report == null) return NotFound();
            return View(new ReportViewModel(report, new DropdownOptions()
            {
                States = await _context.States.OrderBy(s => s.PostalAbbreviation).ToListAsync(),
                Plans = await _context.Plans.OrderBy(p => p.Name).ToListAsync(),
                Frequencies = await _context.Reports.Select(r => r.Frequency).OrderBy(f => f).Distinct().ToListAsync(),
                BusinessContacts = await _context.BusinessContacts.OrderBy(bc => bc.Name).ToListAsync(),
                BusinessOwners = await _context.BusinessContacts.Select(bc => bc.BusinessOwner).OrderBy(bo => bo).Distinct().ToListAsync(),
                SourceDepartments = await _context.Reports.Select(r => r.SourceDepartment).OrderBy(sd => sd).Distinct().ToListAsync()
            }));
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        private string GetCurrentUserID() => _context.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault().Id;

        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Report, Plans")] ReportViewModel reportViewModel /*[Bind("Id,Name,BusinessContact,BusinessOwner,DueDate1,DueDate2,DueDate3,DueDate4,Frequency,DayDue,DeliveryFunction,WorkInstructions,Notes,DaysAfterQuarter,FolderLocation,ReportType,RunWith,DeliveryMethod,DeliveryTo,EffectiveDate,TerminationDate,GroupName,State,ReportPath,OtherDepartment,SourceDepartment,QualityIndicator,ERSReportLocation,ERRStatus,DateAdded,SystemRefreshDate,LegacyReportID,LegacyReportIDR2,ERSReportName,OtherReportLocation,OtherReportName")] Report report*/)
        {
            ToastMessage toast = null;
            if (id != reportViewModel.Report.Id)
            {
                toast = new ToastMessage(message: $"IDs do not match", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                ShowToast(toast);
                return NotFound();
            }
            if (ModelState.IsValid){
                try{
                    var unmodifiedReport = _context.Reports.AsNoTracking().SingleOrDefault(r => r.Id == reportViewModel.Report.Id);                   
                    _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{reportViewModel.Report.Name}\" has been edited.", CompareChanges(unmodifiedReport, reportViewModel.Report)));
                    reportViewModel.Report.BusinessContact = null;
                    List<ReportPlanMap> list = reportViewModel.Plans is null || reportViewModel.Plans.Count() == 0 ? new List<ReportPlanMap>() : reportViewModel.Plans.Select(i => new ReportPlanMap() { PlanId = i, ReportId = reportViewModel.Report.Id }).ToList();              
                    reportViewModel.Report.ReportPlanMapping = list;                    
                    var maps = _context.ReportPlanMapping.Where(m => m.ReportId == reportViewModel.Report.Id);
                    if(maps != null)
                    {
                        _context.ReportPlanMapping.RemoveRange(maps);
                    }
                    if(reportViewModel.Report.ReportPlanMapping != null)
                    {
                        _context.ReportPlanMapping.AddRange(reportViewModel.Report.ReportPlanMapping);
                    }
                    _context.Update(reportViewModel.Report);
                    await _context.SaveChangesAsync();
                    toast = new ToastMessage(message: $"{reportViewModel.Report.Name} has been successfully edited.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
                }
                catch (DbUpdateConcurrencyException){
                    if (!ReportExists(reportViewModel.Report.Id))
                    {
                        toast = new ToastMessage(message: $"{reportViewModel.Report.Name} was not found in the database.", title: "Something Went Wrong...", toasType: ToastEnums.ToastType.Error, options: new ToastOption() { PositionClass = ToastPositions.BottomRight });
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
            return View(reportViewModel);
        }
        
        private void ShowToast(ToastMessage toastMessage)
        {
            _toastNotification.AddToastMessage(toastMessage.Title, toastMessage.Message, toastMessage.ToastType, toastMessage.ToastOptions);
        }

        private string CompareChanges(ReportDeadline old, ReportDeadline updated)
        {
            if (old == null)
            {
                old = new ReportDeadline();
            }
            if (updated == null)
            {
                updated = new ReportDeadline();
            }
            if (old.Equals(updated)) return "No Apparent Changes Made.";
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder
                .Append(Compare("Date Ran", old.RunDate, updated.RunDate))
                .Append(Compare("Date Approved", old.ApprovalDate, updated.ApprovalDate))
                .Append(Compare("Date Sent", old.SentDate, updated.SentDate));
            string Compare<T>(string header, T item1, T item2)
            {
                string item1Entry = item1 != null ? item1.ToString() : "null";
                string item2Entry = item2 != null ? item2.ToString() : "null"; ;
                return (!item1Entry.Equals(item2Entry)) ? ($"{header}: {item1Entry} updated to {item2Entry}\n") : (String.Empty);
            }
            return messageBuilder.ToString();
        }
        private string CompareChanges(Report old, Report updated)
        {
            if (old == null)
            {
                old = new Report();
            }
            if (updated == null)
            {
                updated = new Report();
            }
            if (old.Equals(updated)) return "No Apparent Changes Made.";
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder
                .Append(Compare("Name", old.Name, updated.Name))
                .Append(Compare("Business Contact", old.BusinessContact, updated.BusinessContact))
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
                .Append(Compare("Plan(s)", old.ReportPlanMapping, updated.ReportPlanMapping))
                .Append(Compare("Report Path", old.ReportPath, updated.ReportPath))
                .Append(Compare("Other Department", old.IsFromOtherDepartment, updated.IsFromOtherDepartment))
                .Append(Compare("Source Department", old.SourceDepartment, updated.SourceDepartment))
                .Append(Compare("Quality Indicator", old.IsQualityIndicator, updated.IsQualityIndicator))
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
                return (!item1Entry.Equals(item2Entry)) ? ($"{header}: {item1Entry} updated to {item2Entry}\n") : (String.Empty);
            }
            return messageBuilder.ToString();
        }

        // GET: Reports/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.Reports.SingleOrDefaultAsync(m => m.Id == id);
            if (report == null) return NotFound();
            return View(report);
        }

        // POST: Reports/Delete/5
        [Authorize, HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ToastMessage toast = null;
            var report = await _context.Reports.Include(r => r.BusinessContact).Include(r => r.ReportPlanMapping).ThenInclude(rpm => rpm.Plan).ThenInclude(p => p.State).SingleOrDefaultAsync(m => m.Id == id);
            _context.Reports.Remove(report);
            _context.Add(userLogFactory.Build(GetCurrentUserID(), $"\"{report.Name}\" has been deleted."));
            toast = toast = new ToastMessage(message: $"{report.Name} has been successfully deleted.", title: "Success", toasType: ToastEnums.ToastType.Success, options: new ToastOption() { PositionClass = ToastPositions.BottomRight});
            await _context.SaveChangesAsync();
            ShowToast(toast);
            return RedirectToAction("Index");
        }
        private bool ReportExists(int id) => _context.Reports.Any(e => e.Id == id);
        private bool ReportDeadlineExists(int id) => _context.ReportDeadlines.Any(e => e.Id == id);
    }
}
