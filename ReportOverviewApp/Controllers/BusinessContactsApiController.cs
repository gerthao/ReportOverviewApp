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
    [Route("api/BusinessContacts")]
    [Authorize]
    public class BusinessContactsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessContactsApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        private List<BusinessContact> FilterBusinessContacts(List<BusinessContact> businessContacts, string id, string name, string businessOwner, string sort, int? from, int? take)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int value = 0;
                id = id.Trim();
                if (int.TryParse(id, out value))
                {
                    businessContacts = businessContacts.Where(p => p.Id == value).ToList();
                }
                else
                {
                    id = id.Replace("_", "[0-9]");
                    id = id.Replace("~", "[0-9]+?");
                    Regex r = new Regex("^" + id + "$");
                    businessContacts = businessContacts.Where(p => r.IsMatch(p.Id.ToString())).ToList();
                }
            }
            switch (sort?.ToLower())
            {
                case "id":
                    businessContacts = businessContacts.OrderBy(bc => bc.Id).ToList();
                    break;
                case "businessOwner":
                    businessContacts = businessContacts.OrderBy(bc => bc.BusinessOwner).ToList();
                    break;
                case "name":
                    businessContacts = businessContacts.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    businessContacts = businessContacts.OrderBy(p => p.Id).ToList();
                    break;
            }
            if (!(String.IsNullOrEmpty(name)))
            {
                name = name.ToLower().Trim();
                businessContacts = businessContacts.Where(p => p.Name.ToLower().Contains(name)).ToList();
            }
            if (!(String.IsNullOrEmpty(businessOwner)))
            {
                businessOwner = businessOwner.ToLower().Trim();
                businessContacts = businessContacts.Where(p => p.BusinessOwner.Contains(businessOwner)).ToList();
            }
            if (from != null)
            {
                if (take != null)
                {
                    businessContacts = businessContacts.Skip(from.Value - 1).Take(take.Value).ToList();
                }
                else businessContacts = businessContacts.Skip(from.Value - 1).ToList();
            }
            else
            {
                if (take != null)
                {
                    businessContacts = businessContacts.Take(take.Value).ToList();
                }
            }
            return businessContacts;
        }


        // GET: api/BusinessContactsApi
        [HttpGet]
        public async Task<IEnumerable<BusinessContact>> GetBusinessContacts(string id, string name, string state,  string sort, int? from, int? take)
        {
            var businessContacts = await _context.BusinessContacts.ToListAsync();
            return FilterBusinessContacts(businessContacts, id, name, state, sort, from, take);
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
        [HttpPut("{id}"), ValidateAntiForgeryToken]
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

            return Ok("Changes have been saved.");
        }

        // POST: api/BusinessContactsApi
        [HttpPost, ValidateAntiForgeryToken]
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
        [HttpDelete("{id}"), ValidateAntiForgeryToken]
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