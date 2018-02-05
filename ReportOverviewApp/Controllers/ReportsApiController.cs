using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Reports")]
    [Authorize]
    public class ReportsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<Report> FilterReports(List<Report> reports, string id, string name, string frequency, bool isTermed, string sort, int? from, int? take)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int value = 0;
                id = id.Trim();
                if (int.TryParse(id, out value))
                {
                    reports = reports.Where(p => p.Id == value).ToList();
                }
                else
                {
                    id = id.Replace("_", "[0-9]");
                    id = id.Replace("~", "[0-9]+?");
                    Regex r = new Regex("^" + id + "$");
                    reports = reports.Where(report => r.IsMatch(report.Id.ToString())).ToList();
                }
            }
            switch (sort?.ToLower())
            {
                case "id":
                    reports = reports.OrderBy(report => report.Id).ToList();
                    break;
                case "frequency":
                    reports = reports.OrderBy(report => report.Frequency).ToList();
                    break;
                case "name":
                    reports = reports.OrderBy(report => report.Name).ToList();
                    break;
                case "istermed":
                    reports = reports.OrderByDescending(report => report.IsTermed).ToList();
                    break;
                default:
                    reports = reports.OrderBy(report => report.Id).ToList();
                    break;
            }
            if (!(String.IsNullOrEmpty(name)))
            {
                name = name.ToLower().Trim();
                reports = reports.Where(p => p.Name.ToLower().Contains(name)).ToList();
            }
            if (!(String.IsNullOrEmpty(frequency)))
            {
                frequency = frequency.ToLower().Trim();
                reports = reports.Where(report => report.Frequency.Contains(frequency)).ToList();
            }
            if (from != null)
            {
                if (take != null)
                {
                    reports = reports.Skip(from.Value - 1).Take(take.Value).ToList();
                }
                else reports = reports.Skip(from.Value - 1).ToList();
            }
            else
            {
                if (take != null)
                {
                    reports = reports.Take(take.Value).ToList();
                }
            }
            return reports;
        }

        // GET: api/ReportsApi
        [HttpGet]
        public async Task<IEnumerable<Report>> GetReports(string id, string name, string frequency, bool isTermed, string sort, int? from, int? take)
        {
            var reports = await _context.Reports.ToListAsync();
            return FilterReports(reports, id, name, frequency, isTermed, sort, from, take);
        }

        // GET: api/ReportsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.Id == id);

            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }
        [HttpGet("{id}/BusinessContact"), Route("api/Reports/{id}/BusinessContact")]
        public async Task<IActionResult> GetBusinessContact([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var report = await _context.Reports.FindAsync(id);
            var businessContact = await _context.BusinessContacts.Where(bc => bc.Id == report.Id).SingleOrDefaultAsync();
            return Ok(businessContact);
        }
        [HttpGet("{id}/Plans"), Route("api/Reports/{id}/Plans")]
        public async Task<IActionResult> GetPlans([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mapping = await _context.ReportPlanMapping.Where(rpm => rpm.ReportId == id).Select(rpm => rpm.PlanId).ToListAsync();
            List<Plan> plans = mapping.Select(async i => await _context.Plans.FindAsync(i)).Select(e => e.Result).ToList();
            return Ok(plans);
        }
        [HttpGet("{id}/ReportPlanMapping"), Route("api/Reports/{id}/ReportPlanMapping")]
        public async Task<IActionResult> GetReportPlanMapping([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mapping = await _context.ReportPlanMapping.Where(rpm => rpm.ReportId == id).ToListAsync();
            return Ok(mapping);
        }
        [HttpGet("{id}/Deadlines"), Route("api/Reports/{id}/Deadlines")]
        public async Task<IActionResult> GetDeadlines([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var plans = await _context.ReportDeadlines.Where(rd => rd.ReportId == id).ToListAsync();
            return Ok(plans);
        }

        // PUT: api/Reports/5
        [HttpPut("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> PutReport([FromRoute] int id, [FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.Id)
            {
                return BadRequest();
            }
            foreach(ReportPlanMap rpm in _context.ReportPlanMapping)
            {
                if(rpm.ReportId == id)
                {
                    _context.Entry(rpm).State = EntityState.Deleted;
                }
            }
            foreach(ReportPlanMap rpm in report.ReportPlanMapping)
            {
                _context.Entry(rpm).State = EntityState.Added;
            }
            _context.Entry(report).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Json(report);
        }

        // POST: api/Reports
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReport([FromBody] Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            foreach (ReportPlanMap rpm in report.ReportPlanMapping)
            {
                rpm.ReportId = report.Id;
                _context.Entry(rpm).State = EntityState.Added;
            }

            return CreatedAtAction("GetReport", new { id = report.Id }, report);
        }

        // DELETE: api/Reports/5
        [HttpDelete("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = await _context.Reports.SingleOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return Ok(report);
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}