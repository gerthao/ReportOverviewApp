using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportOverviewApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        ///  Gets a Report based on its ID in JSON format.
        /// </summary>
        /// <param name="id">
        ///  "id" must be a positive integer.
        /// </param>
        /// <returns>
        ///  Returns JsonResult containing a JSON-formatted report.
        /// </returns>
        [Authorize]
        public async Task<JsonResult> GetReport(int? id) => Json(await _context.Reports.Where(r => r.Id == id).ToListAsync());
        [Authorize]
        public async Task<JsonResult> GetReports(int? id_1, int? id_2) => Json(await _context.Reports.Where(r => r.Id >= id_1 && r.Id <= id_2).ToListAsync());
        [Authorize]
        public async Task<JsonResult> GetAllReports() => Json(await _context.Reports.ToListAsync());

        /// <summary>
        /// Returns JSON data of report names and deadlines.
        /// </summary>
        /// <param name="days">
        /// Default is null which returns all ReportFragments.  Else returns the ReportFragments that have a deadline on days added onto the current date.
        /// </param>
        /// <returns>Returns JSON data of report names and deadlines.</returns>
        [Authorize]
        public async Task<JsonResult> GetDeadlines(double? days)
        {
            if (days == null)
            {
                return Json(await _context.Reports.Select(r => new ReportFragment(r)).ToListAsync());
            }
            return Json(await _context.Reports.Select(r => new ReportFragment(r)).Where(r => r.ReportDeadline == DateTime.Today.AddDays(days.Value)).ToListAsync());
        }
        /// <summary>
        /// Returs JSON-formatted data of UserLogs.
        /// </summary>
        /// <returns>
        /// Returs JSON-formatted data of UserLogs.
        /// </returns>
        [Authorize]
        public async Task<JsonResult> GetUserLogs()
        {
            return Json(await _context.UserLogs.ToListAsync());
        }
        public class ReportFragment
        {
            public string ReportName { get; private set; }
            public DateTime? ReportDeadline { get; private set; }
            public ReportFragment(Report report)
            {
                ReportName = report.Name;
                ReportDeadline = report.CurrentDeadline();
            }
        }
    }
}
