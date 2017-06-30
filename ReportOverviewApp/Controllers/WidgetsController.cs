using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models.WidgetModels;

namespace ReportOverviewApp.Controllers
{
    public class WidgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WidgetsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Widgets
        public async Task<IActionResult> Index()
        {
            if(_context.Widget == null)
            {
                return View(Default());
            } if(_context.Widget.Count() == 0)
            {
                return View(Default());
            }
            return View(await _context.Widget.ToListAsync());
        }


        private List<IWidget> Default()
        {
            WidgetBuilder builder = new WidgetBuilder();
            var wigetContainer = new List<IWidget>
            {
                builder
                    .BuildProduct()
                    .BuildID(1)
                    .BuildHeader("Wiget 1")
                    .BuildSubWidgetTopic("All Reports")
                    .BuildSubWidgetAction(WidgetFunctions.ReportCount())
                    .BuildFooter("Footer")
                    .BuildColor("Red")
                    .BuildOption("Edit This Widget")
                    .ReleaseProduct(),

                builder
                    .BuildProduct()
                    .BuildID(2)
                    .BuildHeader("Wiget 2")
                    .BuildSubWidgetTopic("All Reports")
                    .BuildSubWidgetAction(WidgetFunctions.ReportCount(DateTime.Now, DateTime.Now.AddDays(20)))
                    .BuildFooter("Footer")
                    .BuildColor("Black")
                    .BuildOption("Edit This Widget").BuildOption("Delete This Widget")
                    .ReleaseProduct()
            };
            return wigetContainer;
        }


        // GET: Widgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widget
                .SingleOrDefaultAsync(m => m.ID == id);
            if (widget == null)
            {
                return NotFound();
            }

            return View(widget);
        }

        // GET: Widgets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Widgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Color,Header,Footer")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                _context.Add(widget);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(widget);
        }

        // GET: Widgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widget.SingleOrDefaultAsync(m => m.ID == id);
            if (widget == null)
            {
                return NotFound();
            }
            return View(widget);
        }

        // POST: Widgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Color,Header,Footer")] Widget widget)
        {
            if (id != widget.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(widget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WidgetExists(widget.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(widget);
        }

        // GET: Widgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var widget = await _context.Widget
                .SingleOrDefaultAsync(m => m.ID == id);
            if (widget == null)
            {
                return NotFound();
            }

            return View(widget);
        }

        // POST: Widgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var widget = await _context.Widget.SingleOrDefaultAsync(m => m.ID == id);
            _context.Widget.Remove(widget);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool WidgetExists(int id)
        {
            return _context.Widget.Any(e => e.ID == id);
        }
    }
}
