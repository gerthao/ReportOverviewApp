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
using ReportOverviewApp.Data;
using ReportOverviewApp.Helpers;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.ReportViewModels;

namespace ReportOverviewApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Reports/Deadlines")]
    [Authorize]
    public class ReportDeadlinesApiController : Controller
    {
        private ApplicationDbContext _context;
        private UserLogFactory userLogFactory;
        public ReportDeadlinesApiController(ApplicationDbContext context)
        {
            _context = context;
            userLogFactory = new UserLogFactory();
        }
        // GET: api/ReportDeadline
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ReportDeadline/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ReportDeadline
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ReportDeadline/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        [HttpGet]
        public async Task<JsonResult> GetReportDeadlines(int? year, int? month, int? day, DayOfWeek? dayOfWeek, string name, bool indent, bool omitNull)
        {
            var deadlines = await _context.ReportDeadlines.Include(rd => rd.Report).ToListAsync();
            deadlines = (String.IsNullOrEmpty(name) ? deadlines : deadlines.Where(rd => rd.Report.Name.ToLower().Contains(name))).ToList();
            deadlines = (year == null ? deadlines : deadlines.Where(rd => rd.Deadline.Year == year)).ToList();
            deadlines = (month == null ? deadlines : deadlines.Where(rd => rd.Deadline.Month == month)).ToList();
            deadlines = (day == null ? deadlines : deadlines.Where(rd => rd.Deadline.Day == day)).ToList();
            deadlines = (dayOfWeek == null ? deadlines : deadlines.Where(rd => rd.Deadline.DayOfWeek == dayOfWeek)).ToList();
            var result = deadlines.Select(rd => new { rd.Id, rd.Deadline, rd.RunDate, rd.ApprovalDate, rd.SentDate, rd.HasRun, rd.IsApproved, rd.IsSent, rd.ReportId, rd.Report.Name }).ToList();
            return Json(result, new JsonSerializerSettings() { Formatting = indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        [HttpGet("{id}")]
        public async Task<JsonResult> GetReportDeadline(int? id, bool indent, bool omitNull)
        {
            if (id == null) return Json(new { });
            var result = await _context.ReportDeadlines.Include(rd => rd.Report).Where(rd => rd.Id == id).Select(rd =>
                new { rd.Id, rd.Deadline, rd.RunDate, rd.ApprovalDate, rd.SentDate, rd.HasRun, rd.IsApproved, rd.IsSent, rd.ReportId, rd.Report.Name }
            ).SingleOrDefaultAsync();
            return Json(result, new JsonSerializerSettings() { Formatting = indent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None, NullValueHandling = omitNull ? NullValueHandling.Ignore : NullValueHandling.Include });
        }
        //[HttpPost("{dateTime}"), Route("api/Deadlines/MarkAsComplete/{complete}")]
        //public async Task<JsonResult> MarkAll([FromBody] DateTime dateTime,[FromRoute] bool? complete)
        //{
        //    if (dateTime == null || complete == null) return Json(new { success = false, message = "Not Found" });
        //    List<ReportDeadline> list = await _context.ReportDeadlines.Where(rd => rd.Deadline.ToString("MM/dd/yyyy") == dateTime.ToString("MM/dd/yyyy")).ToListAsync();
        //    if (list == null || !list.Any()) return Json(new { success = false, message = "Empty" });
        //    string returnMessage;
        //    if (complete.Value)
        //    {
        //        for (int i = 0; i < list.Count(); i++)
        //        {
        //            if (list[i].ApprovalDate == null)
        //            {
        //                list[i].ApprovalDate = DateTime.Today;
        //            }
        //            if (list[i].RunDate == null)
        //            {
        //                list[i].RunDate = DateTime.Today;
        //            }
        //            if (list[i].SentDate == null)
        //            {
        //                list[i].SentDate = DateTime.Today;
        //            }
        //        }
        //        returnMessage = $"All deadlines for {dateTime.ToString("MM/dd/yyyy")} have been marked as complete";
        //    }
        //    else
        //    {
        //        for (int i = 0; i < list.Count(); i++)
        //        {
        //            list[i].ApprovalDate = null;
        //            list[i].RunDate = null;
        //            list[i].SentDate = null;
        //        }
        //        returnMessage = $"All deadlines for {dateTime.ToString("MM/dd/yyyy")} have been marked as incomplete";
        //    }

        //    _context.ReportDeadlines.UpdateRange(list);
        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true, message = returnMessage });
        //}
        [HttpDelete("{dateTime}"), Route("api/Reports/Deadlines/DeleteAll/{dateTime}")]
        public async Task<IActionResult> DeleteAll([FromBody] DateTime dateTime)
        {
            if (dateTime == null) return BadRequest();
            List<ReportDeadline> list = await _context.ReportDeadlines.Where(rd => rd.Deadline.ToString("MM/dd/yyyy") == dateTime.ToString("MM/dd/yyyy")).ToListAsync();
            if (list == null || !list.Any()) return NotFound();
            _context.ReportDeadlines.RemoveRange(list);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"All deadlines for {dateTime.ToString("MM/dd/yyyy")} have been deleted" });
        }




        [HttpDelete("{id}"), ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> DeleteDeadline([FromRoute] int? id)
        {
            if (id == null) return BadRequest("id parameter did not have a value");
            var reportDeadline = await _context.ReportDeadlines.FindAsync(id);
            if (reportDeadline == null) return NotFound("Value of id parameter not found in data table");
            _context.ReportDeadlines.Remove(reportDeadline);
            await _context.SaveChangesAsync();
            return Json(new { success = true, update = true, message = "Deletion successful" });
        }
        [HttpPut("{id}"), ValidateAntiForgeryToken, Authorize]
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
                    await _context.AddAsync(userLogFactory.Build(GetCurrentUserID(), $"Status of \"{unmodifiedDeadline.Report.Name}\" for {reportDeadline.Deadline.ToString("MM/dd/yyyy")} has been updated.", CompareChanges(unmodifiedDeadline, reportDeadline)));
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
