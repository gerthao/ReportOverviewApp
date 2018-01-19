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
    [Route("api/BusinessContacts")]
    public class BusinessContactsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessContactsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BusinessContactsApi
        [HttpGet]
        public IEnumerable<BusinessContact> GetBusinessContacts()
        {
            return _context.BusinessContacts;
        }

        // GET: api/BusinessContactsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessContact([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var businessContact = await _context.BusinessContacts.SingleOrDefaultAsync(m => m.Id == id);

            if (businessContact == null)
            {
                return NotFound();
            }

            return Ok(businessContact);
        }

        // PUT: api/BusinessContactsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessContact([FromRoute] int id, [FromBody] BusinessContact businessContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != businessContact.Id)
            {
                return BadRequest();
            }

            _context.Entry(businessContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessContactExists(id))
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

        // POST: api/BusinessContactsApi
        [HttpPost]
        public async Task<IActionResult> PostBusinessContact([FromBody] BusinessContact businessContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BusinessContacts.Add(businessContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBusinessContact", new { id = businessContact.Id }, businessContact);
        }

        // DELETE: api/BusinessContactsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessContact([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var businessContact = await _context.BusinessContacts.SingleOrDefaultAsync(m => m.Id == id);
            if (businessContact == null)
            {
                return NotFound();
            }

            _context.BusinessContacts.Remove(businessContact);
            await _context.SaveChangesAsync();

            return Ok(businessContact);
        }

        private bool BusinessContactExists(int id)
        {
            return _context.BusinessContacts.Any(e => e.Id == id);
        }
    }
}