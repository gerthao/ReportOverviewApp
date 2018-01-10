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
using ReportOverviewApp.Models.BusinessContactViewModels;

namespace ReportOverviewApp.Controllers
{
    [Authorize]
    public class BusinessContactsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string unassigned = "Unassigned";
        private const int unassignedId = 0;
        public BusinessContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TransferReports(int? first, int? second)
        {
            //var firstTransfer = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == recipientId);
            //if (owner == null || recipient == null) return NotFound();
            TransferReportsViewModel viewModel = viewModel = new TransferReportsViewModel()
            {
                BusinessContacts = await _context.BusinessContacts.Include(bc => bc.Reports).ToListAsync(),
                FirstReports = new List<int>(),
                SecondReports = new List<int>()
            };
            if (first != null && first.HasValue)
            {
                viewModel.First = first.Value;
            }
            if (second != null && second.HasValue)
            {
                viewModel.Second = second.Value;
            }
            viewModel.BusinessContacts.Add(new BusinessContact() { Name = unassigned, Id = unassignedId, Reports = await _context.Reports.Where(r => r.BusinessContactId == null || !r.BusinessContactId.HasValue).ToListAsync()});
            return View(viewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferReports([FromBody] TransferReportsViewModel viewModel)
        {
            if (viewModel == null) return NotFound();
            if (ModelState.IsValid)
            {
                var firstReportsToTransfer = await _context.Reports.Where(r => r.BusinessContactId != viewModel.First && viewModel.FirstReports.Contains(r.Id)).ToListAsync();
                var secondReportsToTransfer = await _context.Reports.Where(r => r.BusinessContactId != viewModel.Second && viewModel.SecondReports.Contains(r.Id)).ToListAsync();
                if((firstReportsToTransfer == null || !firstReportsToTransfer.Any()) && (secondReportsToTransfer == null || !secondReportsToTransfer.Any()))
                {
                    return Json(new { success = true, update = false, message = "No changes detected." });
                }
                for (int i = 0; i < firstReportsToTransfer.Count(); i++)
                {
                    firstReportsToTransfer[i].BusinessContactId = (viewModel.First == unassignedId ? null : viewModel.First as int?);
                }
                for (int i = 0; i < secondReportsToTransfer.Count(); i++)
                {
                    secondReportsToTransfer[i].BusinessContactId = (viewModel.Second == unassignedId ? null : viewModel.Second as int?);
                }
                _context.UpdateRange(firstReportsToTransfer);
                _context.UpdateRange(secondReportsToTransfer);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(ModelState);
            }
            //var owner = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == ownerId);
            //var recipient = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == recipientId);
            //if (owner == null || recipient == null) return NotFound();
            return Json(new { success = true, update = true, message = "Save was successful"});
        }


        // GET: BusinessContacts
        public async Task<IActionResult> Index(string sort)
        {
            var businessContacts = await _context.BusinessContacts.ToListAsync();
            switch (sort?.ToLower())
            {
                case "id":
                    businessContacts = businessContacts.OrderBy(p => p.Id).ToList();
                    break;
                case "name":
                    businessContacts = businessContacts.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    businessContacts = businessContacts.OrderBy(p => p.Id).ToList();
                    break;

            }
            return View(businessContacts);
        }

        // GET: BusinessContacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessContact = await _context.BusinessContacts.Include(bc => bc.Reports)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (businessContact == null)
            {
                return NotFound();
            }

            return View(businessContact);
        }

        // GET: BusinessContacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusinessContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,BusinessOwner")] BusinessContact businessContact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(businessContact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(businessContact);
        }

        // GET: BusinessContacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessContact = await _context.BusinessContacts.SingleOrDefaultAsync(m => m.Id == id);
            if (businessContact == null)
            {
                return NotFound();
            }
            return View(businessContact);
        }

        // POST: BusinessContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,BusinessOwner")] BusinessContact businessContact)
        {
            if (id != businessContact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(businessContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusinessContactExists(businessContact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(businessContact);
        }

        // GET: BusinessContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var businessContact = await _context.BusinessContacts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (businessContact == null)
            {
                return NotFound();
            }

            return View(businessContact);
        }

        // POST: BusinessContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var businessContact = await _context.BusinessContacts.SingleOrDefaultAsync(m => m.Id == id);
            _context.BusinessContacts.Remove(businessContact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusinessContactExists(int id)
        {
            return _context.BusinessContacts.Any(e => e.Id == id);
        }
    }
}
