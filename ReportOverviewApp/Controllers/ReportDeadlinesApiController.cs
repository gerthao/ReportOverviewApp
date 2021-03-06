﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.ReportViewModels;

namespace ReportOverviewApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Deadlines")]
    [Authorize]
    public class ReportDeadlinesApiController : Controller
    {
        private ApplicationDbContext _context;
        public ReportDeadlinesApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<JsonResult> GetReportDeadlines(int? year, int? month, int? day, DayOfWeek? dayOfWeek, string report, string plan, string groupBy, bool indent, bool omitNull)
        {
            var deadlines = await _context.ReportDeadlines.Include(rd => rd.Report)
                                                                .ThenInclude(r => r.ReportPlanMapping)
                                                                    .ThenInclude(rpm => rpm.Plan)
                                                                        .ThenInclude(p => p.State)
                                                          .OrderBy(rd => rd.Deadline).ToListAsync();
            deadlines = (String.IsNullOrEmpty(report) ? deadlines : deadlines.Where(rd => rd.Report.Name.ToLower().Contains(report.ToLower()))).ToList();
            deadlines = (year == null ? deadlines : deadlines.Where(rd => rd.Deadline.Year == year)).ToList();
            deadlines = (month == null ? deadlines : deadlines.Where(rd => rd.Deadline.Month == month)).ToList();
            deadlines = (day == null ? deadlines : deadlines.Where(rd => rd.Deadline.Day == day)).ToList();
            deadlines = (dayOfWeek == null ? deadlines : deadlines.Where(rd => rd.Deadline.DayOfWeek == dayOfWeek)).ToList();
            deadlines = (String.IsNullOrEmpty(plan) ? deadlines : deadlines.Where(rd => rd.Report.ReportPlanMapping.Select(rpm => rpm.Plan).Where(p => p.Name.ToLower().Contains(plan.ToLower())).Any())).ToList();
            var result = deadlines.Select(rd => new
                                {
                                     rd.Id, rd.Deadline, rd.RunDate, rd.ApprovalDate, rd.SentDate, rd.HasRun, rd.IsApproved, rd.IsSent, rd.IsComplete, rd.IsLate, rd.IsPastDue, rd.ReportId,
                                        Report = new
                                        {
                                            rd.Report.Id,
                                            rd.Report.Name,
                                            rd.Report.Frequency,
                                            Plans = rd.Report.ReportPlanMapping.Select(
                                                rpm => new
                                                {
                                                    rpm.Plan.Id,
                                                    rpm.Plan.Name,
                                                    State = new
                                                    {
                                                        rpm.Plan.State.Id,
                                                        rpm.Plan.State.Name,
                                                        rpm.Plan.State.PostalAbbreviation
                                                    }
                                                })
                                        }
                                }).ToList();
            return Json(result, new JsonSerializerSettings() { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ContractResolver = new CamelCasePropertyNamesContractResolver() ,Formatting = indent ? Formatting.Indented : Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        [HttpPost("Generate"), Route("api/Deadlines/Generate"), ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateDeadlines()
        {
            DateTime date = DateTime.Today;
            int updates = 0;
            List<string> updatedReports = new List<string>();
            for (int i = 0; i < 1; i++)
            {
                DateTime checkDate = date.AddDays(i);
                await _context.Reports.Include(r => r.Deadlines).ForEachAsync(async r =>
                {
                    DateTime? currentCalculatedDeadline = r.Deadline(checkDate);
                    if (currentCalculatedDeadline != null && currentCalculatedDeadline.HasValue)
                    {
                        DateTime? mostRecentReportDeadline = r.Deadlines.OrderByDescending(rd => rd.Deadline).Select(rd => rd.Deadline as DateTime?).FirstOrDefault();
                        if (currentCalculatedDeadline > mostRecentReportDeadline || mostRecentReportDeadline == null)
                        {
                            await _context.ReportDeadlines.AddAsync(new ReportDeadline()
                            {
                                ReportId = r.Id,
                                Deadline = currentCalculatedDeadline.Value
                            });
                            updatedReports.Add(r.Name);
                            updates++;
                        }
                    }
                });
                await _context.SaveChangesAsync();
            }
            await _context.UserLogs.AddAsync(new UserLog(GetCurrentUserID(), $"{updates} new deadlines created.", DateTime.Now));
            await _context.SaveChangesAsync();
            return Json(new { success = true, updates, updatedReports });
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetReportDeadline(int? id, bool indent, bool omitNull)
        {
            if (id == null) return Json(new { });
            var result = await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Id == id).Select(rd =>
                new
                {
                    rd.Id,
                    rd.Deadline,
                    rd.RunDate,
                    rd.ApprovalDate,
                    rd.SentDate,
                    rd.HasRun,
                    rd.IsApproved,
                    rd.IsSent,
                    rd.IsComplete,
                    rd.IsLate,
                    rd.IsPastDue,
                    rd.ReportId,
                    Report = new
                    {
                        rd.Report.Id,
                        rd.Report.Name,
                        rd.Report.Frequency,
                        Plans = rd.Report.ReportPlanMapping.Select(rpm => new { rpm.Plan.Id, rpm.Plan.Name,
                            State = new
                            {
                                rpm.Plan.State.Id,
                                rpm.Plan.State.Name,
                                rpm.Plan.State.PostalAbbreviation
                            }
                        })
                    }
                }
            ).SingleOrDefaultAsync();
            return Json(result, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), Formatting = indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        [HttpDelete("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDeadline([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reportDeadline = await _context.ReportDeadlines.SingleOrDefaultAsync(m => m.Id == id);
            if (reportDeadline == null)
            {
                return NotFound();
            }
            _context.ReportDeadlines.Remove(reportDeadline);
            await _context.SaveChangesAsync();
            return Ok(reportDeadline);
        }
        [HttpPut("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDeadline([FromRoute] int? id, [FromBody] ReportDeadline reportDeadline)
        {
            if (id == null)
            {
                return NotFound("Unable to get an id value.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    reportDeadline.Deadline = new DateTime(year: reportDeadline.Deadline.Year, month: reportDeadline.Deadline.Month, day: reportDeadline.Deadline.Day);
                    var unmodifiedDeadline = _context.ReportDeadlines.AsNoTracking().Include(rd => rd.Report).SingleOrDefault(d => d.Id == id);
                    await _context.AddAsync(new UserLog(GetCurrentUserID(), $"Deadline of \"{unmodifiedDeadline.Report.Name}\" for {reportDeadline.Deadline.ToString("MM/dd/yyyy")} has been updated.", DateTime.Now, CompareChanges(unmodifiedDeadline, reportDeadline)));
                    Report report = await _context.Reports.FindAsync(reportDeadline.ReportId);
                    if (report == null) throw new NullReferenceException($"Report is null and given {reportDeadline.ReportId} as Id for deadline of {reportDeadline.Deadline} with Id {reportDeadline.Id}");
                    _context.ReportDeadlines.Update(reportDeadline);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportDeadlineExists(reportDeadline.Id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
                return Json(new { success = true, update = true, message = "Save successful" });
            }
            return BadRequest(ModelState);
        }
        private bool ReportDeadlineExists(int id) => _context.ReportDeadlines.Any(e => e.Id == id);
        private string GetCurrentUserID() => _context.Users.Where(u => u.UserName.Equals(User.Identity.Name)).SingleOrDefault().Id;
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
    }
}
