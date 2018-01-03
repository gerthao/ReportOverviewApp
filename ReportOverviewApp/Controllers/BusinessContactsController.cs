using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using ReportOverviewApp.Models.BusinessContactViewModels;

namespace ReportOverviewApp.Controllers
{
    public class BusinessContactsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> TransferReports()
        {
            //var recipient = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == recipientId);
            //if (owner == null || recipient == null) return NotFound();
            var viewModel = new TransferReportsViewModel()
            {
                BusinessContacts = await _context.BusinessContacts.Include(bc => bc.Reports).ToListAsync(),
                FirstReports = new List<int>(),
                SecondReports = new List<int>()
            };
            return View(viewModel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferReports([FromBody] TransferReportsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var firstReportsToTransfer = viewModel.FirstReports.Select(i => _context.Reports.Find(i)).ToList();
            
                for (int i = 0; i < firstReportsToTransfer.Count(); i++)
                {
                    firstReportsToTransfer[i].BusinessContactId = viewModel.First;
                }
                _context.UpdateRange(firstReportsToTransfer);
                await _context.SaveChangesAsync();
                var secondReportsToTransfer = viewModel.SecondReports.Select(i =>  _context.Reports.Find(i)).ToList();
                for (int i = 0; i < secondReportsToTransfer.Count(); i++)
                {
                    secondReportsToTransfer[i].BusinessContactId = viewModel.Second;
                }
                _context.UpdateRange(secondReportsToTransfer);
                await _context.SaveChangesAsync();
            } else
            {
                return BadRequest(ModelState);
            }
            //var owner = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == ownerId);
            //var recipient = await _context.BusinessContacts.Include(bc => bc.Reports).SingleOrDefaultAsync(bc => bc.Id == recipientId);
            //if (owner == null || recipient == null) return NotFound();
            return Json(new { success = true });
        }


        // GET: BusinessContacts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BusinessContacts.ToListAsync());
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
