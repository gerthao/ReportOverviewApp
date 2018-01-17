using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Plans
        
        [HttpGet, Route("Plans")]
        public async Task<IActionResult> Index(int? id, string name, string sort)
        {
            //var plans = await _context.Plans.Include(p => p.State).Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report).ToListAsync();
            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["sort"] = sort;
            return View();
        }

        [HttpGet, Route("api/Plans")]
        public async Task<JsonResult> GetPlans(int? id, string name, string sort, int? from, int? take)
        {
            var plans = await _context.Plans.Include(p => p.State).ToListAsync();
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
            if(id != null)
            {
                plans = plans.Where(p => p.Id == id).ToList();
            }
            if (name != null)
            {
                plans = plans.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            if(from != null)
            {
                if (take != null)
                {
                    plans = plans.Skip(from.Value - 1).Take(take.Value).ToList();
                }
                else plans = plans.Skip(from.Value - 1).ToList();
            } else
            {
                if (take != null)
                {
                    plans = plans.Take(take.Value).ToList();
                }
            }
            return Json(plans.Select(p => new { p.Id, p.Name, state = p.State.PostalAbbreviation, p.StateId, p.WindwardId, p.HasActiveReports, p.HasTermedReports }).ToList());
        }


        // GET: Plans/Details/5
        [Route("Plans/Details/{id:int?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.State)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: Plans/Create
        [HttpGet, Route("Plans/Create")]
        public IActionResult Create()
        {
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, Route("api/Plans/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
            return View(plan);
        }

        // GET: Plans/Edit/5
        [HttpGet("{id}"), Route("Plans/{id:int?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id}"), Route("api/Plans/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Plan plan)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true, update = true, message = "Save successful" });
            }
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
            return BadRequest();
        }

        //// GET: Plans/Delete/5
        //[HttpGet("{id}"), Route("Plans/{id:int?}")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plan = await _context.Plans
        //        .Include(p => p.State)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (plan == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(plan);
        //}

        // POST: Plans/Delete/5
        [HttpDelete("{id}"), ActionName("Delete"), Route("api/Plans/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
            if (plan == null) return NotFound();
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return Json(new { success = true, update = true, message = "Deletion successful" });
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}
