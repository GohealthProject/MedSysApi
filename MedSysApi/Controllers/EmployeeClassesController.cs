using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeClassesController : ControllerBase
    {
        private readonly MedSysContext _context;

        public EmployeeClassesController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeClass>>> GetEmployeeClasses()
        {
          if (_context.EmployeeClasses == null)
          {
              return NotFound();
          }
            return await _context.EmployeeClasses.ToListAsync();
        }

        // GET: api/EmployeeClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeClass>> GetEmployeeClass(int id)
        {
          if (_context.EmployeeClasses == null)
          {
              return NotFound();
          }
            var employeeClass = await _context.EmployeeClasses.FindAsync(id);

            if (employeeClass == null)
            {
                return NotFound();
            }

            return employeeClass;
        }

        // PUT: api/EmployeeClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeClass(int id, EmployeeClass employeeClass)
        {
            if (id != employeeClass.EmployeeClassId)
            {
                return BadRequest();
            }

            _context.Entry(employeeClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeClassExists(id))
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

        // POST: api/EmployeeClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeClass>> PostEmployeeClass(EmployeeClass employeeClass)
        {
          if (_context.EmployeeClasses == null)
          {
              return Problem("Entity set 'MedSysContext.EmployeeClasses'  is null.");
          }
            _context.EmployeeClasses.Add(employeeClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeClass", new { id = employeeClass.EmployeeClassId }, employeeClass);
        }

        // DELETE: api/EmployeeClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeClass(int id)
        {
            if (_context.EmployeeClasses == null)
            {
                return NotFound();
            }
            var employeeClass = await _context.EmployeeClasses.FindAsync(id);
            if (employeeClass == null)
            {
                return NotFound();
            }

            _context.EmployeeClasses.Remove(employeeClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeClassExists(int id)
        {
            return (_context.EmployeeClasses?.Any(e => e.EmployeeClassId == id)).GetValueOrDefault();
        }
    }
}
