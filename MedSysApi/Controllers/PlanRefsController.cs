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
    public class PlanRefsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public PlanRefsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/PlanRefs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanRef>>> GetPlanRefs()
        {
          if (_context.PlanRefs == null)
          {
              return NotFound();
          }
            return await _context.PlanRefs.ToListAsync();
        }

        // GET: api/PlanRefs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlanRef>> GetPlanRef(int id)
        {
          if (_context.PlanRefs == null)
          {
              return NotFound();
          }
            var planRef = await _context.PlanRefs.FindAsync(id);

            if (planRef == null)
            {
                return NotFound();
            }

            return planRef;
        }

        // GET: api/PlanRefs/5
        [HttpGet("get/{id}")]
        public IActionResult GPlanRef(int id)
        {
            var q = from p in _context.PlanRefs
                where p.PlanId == id
                select p;

            return Ok(q);
        }

        // PUT: api/PlanRefs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanRef(int id, PlanRef planRef)
        {
            if (id != planRef.PlanbridgeId)
            {
                return BadRequest();
            }

            _context.Entry(planRef).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanRefExists(id))
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
        public IActionResult PutPlanRef(int id)
        {
            var q = Request.Form;
            var pjid = q["prjchk"]; //(陣列)

            var del = from p in _context.PlanRefs
                where p.PlanId == id
                select p;

            _context.PlanRefs.RemoveRange(del);

            foreach (var item in pjid)
            {
                var a = new PlanRef();
                a.PlanId = id;
                a.ProjectId = Int32.Parse(item);

                _context.PlanRefs.Add(a);
            }

            _context.SaveChanges();

            return Ok();
        }

            // POST: api/PlanRefs
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
        public IActionResult PostPlanRef()
        {
            var q = Request.Form;
            var pjid = q["Cprjchk"]; //(陣列)

            //int pid 為 Plan資料表中最後一個欄位的ID
            var pid = _context.Plans.Max(p => p.PlanId);

            foreach (var item in pjid)
            {
                var a = new PlanRef();
                a.PlanId = pid;
                a.ProjectId = Int32.Parse(item);

                _context.PlanRefs.Add(a);
            }

            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/PlanRefs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanRef(int id)
        {
            if (_context.PlanRefs == null)
            {
                return NotFound();
            }
            var planRef = await _context.PlanRefs.FindAsync(id);

            var del = from p in _context.PlanRefs
                where p.PlanId == id
                select p;

            _context.PlanRefs.RemoveRange(del);



            if (planRef == null)
            {
                return NotFound();
            }

            _context.PlanRefs.Remove(planRef);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanRefExists(int id)
        {
            return (_context.PlanRefs?.Any(e => e.PlanbridgeId == id)).GetValueOrDefault();
        }
    }
}
