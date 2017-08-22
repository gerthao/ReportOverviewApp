using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportOverviewApp.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<JsonResult> GetReport(int? id)
        {
            var report = await _context.Reports.Where(r => r.ID == id).ToListAsync();
            return Json(report);
        }
        [Authorize]
        public async Task<JsonResult> GetReports(int? id_1, int? id_2)
        {
            return Json(await _context.Reports.Where(r => r.ID >= id_1 && r.ID <= id_2).ToListAsync());
        }
        [Authorize]
        public async Task<JsonResult> GetAllReportsAsync()
        {
            return Json(await _context.Reports.ToListAsync());
        }
        [Authorize]
        public async Task<JsonResult> GetDeadlines(double? days)
        {
            if (days == null)
            {
                return Json(await _context.Reports.Select(r => r.CurrentDeadline()).ToListAsync());
            }
            return Json(await _context.Reports.Select(r => r.CurrentDeadline()).Where(r => r == DateTime.Today.AddDays(days.Value)).ToListAsync());
        }
        [Authorize]
        public async Task<JsonResult> GetUserLogs()
        {
            return Json(await _context.UserLogs.ToListAsync());
        }
    }
}
