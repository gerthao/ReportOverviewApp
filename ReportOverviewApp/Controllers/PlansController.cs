using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public async Task<IActionResult> Index(string id, string name, string state, string windwardId, string sort, int? page = 1, int? take = 20)
        {
            //var plans = await _context.Plans.Include(p => p.State).Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report).ToListAsync();
            //if (page == null || page < 1)
            //{
            //    page = 1;
            //}
            //if (take == null || take < 1)
            //{
            //    take = 20;
            //}
            //var from = (page - 1) * take;
            //ViewData["max"] = plans.Count() % take == 0 ? plans.Count() / take : (plans.Count() / take) + 1;
            //plans = FilterPlans(plans, id, name, state, windwardId, sort, from, take);
            ViewData["id"] = id;
            ViewData["name"] = name;
            ViewData["sort"] = sort;
            ViewData["windwardId"] = windwardId;
            ViewData["state"] = state;
            ViewData["page"] = page;
            ViewData["take"] = take;
            ViewData["StateId"] = new SelectList(await _context.States.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");

            return View();
        }
        //private List<Plan> FilterPlans(List<Plan> plans, string id, string name, string state, string windwardId, string sort, int? from, int? take)
        //{
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        int value = 0;
        //        id = id.Trim();
        //        if (int.TryParse(id, out value))
        //        {
        //            plans = plans.Where(p => p.Id == value).ToList();
        //        }
        //        else
        //        {
        //            id = id.Replace("_", "[0-9]");
        //            id = id.Replace("~", "[0-9]+?");
        //            Regex r = new Regex("^" + id + "$");
        //            plans = plans.Where(p => r.IsMatch(p.Id.ToString())).ToList();
        //        }
        //    }
        //    switch (sort?.ToLower())
        //    {
        //        case "id":
        //            plans = plans.OrderBy(p => p.Id).ToList();
        //            break;
        //        case "state":
        //            plans = plans.OrderBy(p => p.State.Name).ToList();
        //            break;
        //        case "name":
        //            plans = plans.OrderBy(p => p.Name).ToList();
        //            break;
        //        case "windwardid":
        //            plans = plans.OrderBy(p => p.WindwardId).ToList();
        //            break;
        //        case "hastermedreports":
        //            plans = plans.OrderByDescending(p => p.HasTermedReports).ToList();
        //            break;
        //        case "hasactivereports":
        //            plans = plans.OrderByDescending(p => p.HasActiveReports).ToList();
        //            break;
        //        default:
        //            plans = plans.OrderBy(p => p.Id).ToList();
        //            break;
        //    }
        //    if (!(String.IsNullOrEmpty(name)))
        //    {
        //        name = name.ToLower().Trim();
        //        plans = plans.Where(p => p.Name.ToLower().Contains(name)).ToList();
        //    }
        //    if (!(String.IsNullOrEmpty(state)))
        //    {
        //        state = state.ToLower().Trim();
        //        plans = plans.Where(p => p.State.PostalAbbreviation.ToLower().Contains(state)).ToList();
        //    }
        //    if (!(String.IsNullOrEmpty(windwardId)))
        //    {
        //        windwardId = windwardId.Trim();
        //        int value = 0;
        //        if (int.TryParse(windwardId, out value))
        //        {
        //            plans = plans.Where(p => p.WindwardId.Contains(windwardId)).ToList();
        //        }
        //        else
        //        {
        //            windwardId = windwardId.Replace("_", "[0-9]");
        //            windwardId = windwardId.Replace("~", "[0-9]+?");
        //            Regex r = new Regex("^" + windwardId + "$");
        //            plans = plans.Where(p => r.IsMatch(p.WindwardId)).ToList();
        //        }
        //    }
        //    if (from != null)
        //    {
        //        if (take != null)
        //        {
        //            plans = plans.Skip(from.Value - 1).Take(take.Value).ToList();
        //        }
        //        else plans = plans.Skip(from.Value - 1).ToList();
        //    }
        //    else
        //    {
        //        if (take != null)
        //        {
        //            plans = plans.Take(take.Value).ToList();
        //        }
        //    }
        //    return plans;
        //}
        //[HttpGet, Route("api/Plans"), Produces("application/json")]
        //public async Task<JsonResult> GetPlans(string id, string name, string state, string windwardId, string sort, int? from, int? take)
        //{
        //    var plans = await _context.Plans.Include(p => p.State).Include(p => p.ReportPlanMapping).ThenInclude(rpm => rpm.Report).ToListAsync();
        //    return Json(FilterPlans(plans, id, name, state, windwardId, sort, from, take).Select(p => new { p.Id, p.Name, state = p.State.PostalAbbreviation, p.StateId, p.WindwardId, p.HasActiveReports, p.HasTermedReports }).ToList());
        //}
        //[HttpGet, Route("api/Plans/{id}"), Produces("application/json")]
        //public async Task<JsonResult> GetPlan(int? id)
        //{
        //    var plan = await _context.Plans.FindAsync(id);
        //    return Json(plan);
        //}


        //// GET: Plans/Details/5
        ////[Route("Plans/Details/{id}")]
        ////public async Task<IActionResult> Details(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var plan = await _context.Plans
        ////        .Include(p => p.State)
        ////        .SingleOrDefaultAsync(m => m.Id == id);
        ////    if (plan == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return View(plan);
        ////}

        //// GET: Plans/Create
        //public IActionResult Create()
        //{
        //    ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");
        //    return View();
        //}

        //// POST: Plans/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost, Route("api/Plans/")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([FromBody] Plan plan)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(plan);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
        //    return View(plan);
        //}

        //// GET: Plans/Edit/5
        //[HttpGet, Route("Plans/{id}")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
        //    if (plan == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
        //    return View(plan);
        //}

        //// POST: Plans/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPut("{id}"), Route("api/Plans/{id}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [FromBody] Plan plan)
        //{
        //    if (id != plan.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(plan);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PlanExists(plan.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return Ok();
        //    }
        //    //ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", plan.StateId);
        //    return BadRequest();
        //}


        ////// GET: Plans/Delete/5
        ////[HttpGet("{id}"), Route("Plans/{id:int?}")]
        ////public async Task<IActionResult> Delete(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    var plan = await _context.Plans
        ////        .Include(p => p.State)
        ////        .SingleOrDefaultAsync(m => m.Id == id);
        ////    if (plan == null)
        ////    {
        ////        return NotFound();
        ////    }

        ////    return View(plan);
        ////}

        //// POST: Plans/Delete/5
        //[HttpDelete("{id}"), ActionName("Delete"), Route("api/Plans/{id}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var plan = await _context.Plans.SingleOrDefaultAsync(m => m.Id == id);
        //    if (plan == null) return NotFound();
        //    _context.Plans.Remove(plan);
        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true, update = true, message = "Deletion successful" });
        //}

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}
