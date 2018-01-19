using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Plans")]
    public class PlansApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlansApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<Plan> FilterPlans(List<Plan> plans, string id, string name, string state, string windwardId, string sort, int? from, int? take)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int value = 0;
                id = id.Trim();
                if (int.TryParse(id, out value))
                {
                    plans = plans.Where(p => p.Id == value).ToList();
                }
                else
                {
                    id = id.Replace("_", "[0-9]");
                    id = id.Replace("~", "[0-9]+?");
                    Regex r = new Regex("^" + id + "$");
                    plans = plans.Where(p => r.IsMatch(p.Id.ToString())).ToList();
                }
            }
            switch (sort?.ToLower())
            {
                case "id":
                    plans = plans.OrderBy(p => p.Id).ToList();
                    break;
                case "state":
                    plans = plans.OrderBy(p => p.State.Name).ToList();
                    break;
                case "name":
                    plans = plans.OrderBy(p => p.Name).ToList();
                    break;
                case "windwardid":
                    plans = plans.OrderBy(p => p.WindwardId).ToList();
                    break;
                case "hastermedreports":
                    plans = plans.OrderByDescending(p => p.HasTermedReports).ToList();
                    break;
                case "hasactivereports":
                    plans = plans.OrderByDescending(p => p.HasActiveReports).ToList();
                    break;
                default:
                    plans = plans.OrderBy(p => p.Id).ToList();
                    break;
            }
            if (!(String.IsNullOrEmpty(name)))
            {
                name = name.ToLower().Trim();
                plans = plans.Where(p => p.Name.ToLower().Contains(name)).ToList();
            }
            if (!(String.IsNullOrEmpty(state)))
            {
                state = state.ToLower().Trim();
                plans = plans.Where(p => p.State.PostalAbbreviation.ToLower().Contains(state)).ToList();
            }
            if (!(String.IsNullOrEmpty(windwardId)))
            {
                windwardId = windwardId.Trim();
                int value = 0;
                if (int.TryParse(windwardId, out value))
                {
                    plans = plans.Where(p => p.WindwardId.Contains(windwardId)).ToList();
                }
                else
                {
                    windwardId = windwardId.Replace("_", "[0-9]");
                    windwardId = windwardId.Replace("~", "[0-9]+?");
                    Regex r = new Regex("^" + windwardId + "$");
                    plans = plans.Where(p => r.IsMatch(p.WindwardId)).ToList();
                }
            }
            if (from != null)
            {
                if (take != null)
                {
                    plans = plans.Skip(from.Value - 1).Take(take.Value).ToList();
                }
                else plans = plans.Skip(from.Value - 1).ToList();
            }
            else
            {
                if (take != null)
                {
                    plans = plans.Take(take.Value).ToList();
                }
            }
            return plans;
        }
        [HttpGet]
        public async Task<JsonResult> GetPlans(string id, string name, string state, string windwardId, string sort, int? from, int? take)
        {
            var plans = await _context.Plans.Include(p => p.State).Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report).ToListAsync();
            return Json(FilterPlans(plans, id, name, state, windwardId, sort, from, take).Select(p => new { p.Id, p.Name, state = p.State.PostalAbbreviation, p.StateId, p.WindwardId, p.HasActiveReports, p.HasTermedReports }).ToList());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlan(int? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null) return NotFound();
            return Ok(plan);
        }
        [HttpGet("{id}/State"), Route("api/Plans/{id}/State")]
        public async Task<IActionResult> GetState(int? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null) return NotFound();
            var state = await _context.States.FindAsync(plan.StateId);
            state.Plans = null;
            return Ok(state);
        }
        [HttpGet("{id}/Reports"), Route("api/Plans/{id}/Reports")]
        public async Task<IActionResult> GetReports(int? id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var mapping = await _context.Plans.Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report).Where(p => p.Id == id).Select(p => p.ReportPlanMapping).SingleOrDefaultAsync();
            if (mapping == null) return NotFound();
            var reports = mapping.ToList().Select(rpm => rpm.Report).ToList();
            for(int i = 0; i < reports.Count(); i++)
            {
                reports[i].ReportPlanMapping = null;
            }
            return Ok(reports);
        }

        // PUT: api/PlansApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlan([FromRoute] int id, [FromBody] Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != plan.Id)
            {
                return BadRequest();
            }

            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Changes have been saved");
        }

        // POST: api/PlansApi
        [HttpPost]
        public async Task<IActionResult> PostPlan([FromBody] Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlan", new { id = plan.Id }, plan);
        }

        // DELETE: api/PlansApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            return Ok(plan);
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}