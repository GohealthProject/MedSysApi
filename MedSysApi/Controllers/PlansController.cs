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
    public class PlansController : ControllerBase
    {
        private readonly MedSysContext _context;

        public PlansController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Plans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            if (_context.Plans == null)
            {
                return NotFound();
            }
            return await _context.Plans.ToListAsync();
        }

        // GET: api/Plans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            if (_context.Plans == null)
            {
                return NotFound();
            }
            var plan = await _context.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return plan;
        }

        [HttpGet("test/{id}")]
        public async Task<ActionResult<Plan>> GPlan(int id)
        {
            var q = from p in _context.Plans
                    select p;


            return Ok(q);

        }


        // PUT: api/Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlan(int id, Plan plan)
        {
            if (id != plan.PlanId)
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

            return NoContent();
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Up/{id}")]
        public IActionResult PutPlan(int id)
        {
            var q = Request.Form;

            string pjname = q["PlanName"];
            string pjdesc = q["PlanDescription"];

            var plan = _context.Plans.Where(p => p.PlanId == id).FirstOrDefault();
            plan.PlanName = pjname;
            plan.PlanDescription = pjdesc;

            _context.SaveChanges();
            return Ok();
        }

        // POST: api/Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostPlan()
        {
            var q = Request.Form;
            //var pjid = q["Cprjchk"];

            string pjname = q["CPlanName"];
            string pjdesc = q["CPlanDescription"];

            var plan = new Plan()
            {
                PlanName = pjname,
                PlanDescription = pjdesc,
            };

            _context.Plans.Add(plan);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            //先刪除PlanRefs中的資料
            var q = _context.PlanRefs.Where(p => p.PlanId == id);
            foreach (var item in q)
            {
                _context.PlanRefs.Remove(item);
            }
            await _context.SaveChangesAsync();

            //再刪除Plans中的資料
            if (_context.Plans == null)
            {
                return NotFound();
            }
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanExists(int id)
        {
            return (_context.Plans?.Any(e => e.PlanId == id)).GetValueOrDefault();
        }
    }
}
