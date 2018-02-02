using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Controllers
{
    [Produces("application/json")]
    [Route("api/ReportPlanMapping")]
    public class ReportPlanMappingApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportPlanMappingApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ReportPlanMappingApi
        [HttpGet]
        public IEnumerable<ReportPlanMap> GetReportPlanMapping()
        {
            return _context.ReportPlanMapping;
        }

        // GET: api/ReportPlanMappingApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportPlanMap([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reportPlanMap = await _context.ReportPlanMapping.SingleOrDefaultAsync(m => m.Id == id);

            if (reportPlanMap == null)
            {
                return NotFound();
            }

            return Ok(reportPlanMap);
        }

        // PUT: api/ReportPlanMappingApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReportPlanMap([FromRoute] int id, [FromBody] ReportPlanMap reportPlanMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportPlanMap.Id)
            {
                return BadRequest();
            }

            _context.Entry(reportPlanMap).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportPlanMapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReportPlanMappingApi
        [HttpPost]
        public async Task<IActionResult> PostReportPlanMap([FromBody] ReportPlanMap reportPlanMap)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ReportPlanMapping.Add(reportPlanMap);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReportPlanMap", new { id = reportPlanMap.Id }, reportPlanMap);
        }

        // DELETE: api/ReportPlanMappingApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReportPlanMap([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reportPlanMap = await _context.ReportPlanMapping.SingleOrDefaultAsync(m => m.Id == id);
            if (reportPlanMap == null)
            {
                return NotFound();
            }

            _context.ReportPlanMapping.Remove(reportPlanMap);
            await _context.SaveChangesAsync();

            return Ok(reportPlanMap);
        }

        private bool ReportPlanMapExists(int id)
        {
            return _context.ReportPlanMapping.Any(e => e.Id == id);
        }
    }
}