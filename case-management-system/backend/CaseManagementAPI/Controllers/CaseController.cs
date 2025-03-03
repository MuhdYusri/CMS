using Microsoft.AspNetCore.Mvc;
using CaseManagementAPI.Data;
using CaseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseManagementAPI.Controllers
{
    [Route("api/cases")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CaseController(AppDbContext context)
        {
            _context = context;
        }

        // Get all cases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCase>>> GetCases()
        {
            return await _context.CustomerCases.ToListAsync();
        }

        // Get case by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerCase>> GetCase(int id)
        {
            var customerCase = await _context.CustomerCases.FindAsync(id);
            if (customerCase == null) return NotFound();
            return customerCase;
        }

        // Add new case
        [HttpPost]
        public async Task<ActionResult<CustomerCase>> PostCase(CustomerCase customerCase)
        {
            _context.CustomerCases.Add(customerCase);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCase), new { id = customerCase.Id }, customerCase);
        }

        // Update existing case
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCase(int id, CustomerCase customerCase)
        {
            if (id != customerCase.Id) return BadRequest();
            _context.Entry(customerCase).State = EntityState.Modified;
            customerCase.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete case
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var customerCase = await _context.CustomerCases.FindAsync(id);
            if (customerCase == null) return NotFound();
            _context.CustomerCases.Remove(customerCase);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
