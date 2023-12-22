using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using Microsoft.EntityFrameworkCore.Storage;

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
            //IDbContextTransaction dbContextTransaction = null;

            try
            {
                

               //dbContextTransaction = _context.Database.BeginTransaction();
               

                var q = Request.Form;
                //var pjid = q["Cprjchk"];

                var files = Request.Form.Files;

                string plname = q["CPlanName"];
                string pldesc = q["CPlanDescription"];
                string pling = files[0].FileName;
                var Cprjtxt = q["Cprjtxt"]; //陣列
                var pjid = q["Cprjchk"];

                //先加方案
                var plan = new Plan()
                {
                    PlanName = plname,
                    PlanDescription = pldesc,
                    PlanImg = pling
                };
                _context.Plans.Add(plan);

                //-----------------------------------------------------
                ////再加方案專案關聯
                //foreach (var item in Cprjtxt)
                //{
                //    if (item != null)
                //    {
                //        var pjname = new Project();
                //        pjname.ProjectName = item;
                //        pjname.ProjectPrice = 0;

                //        pjname.ProjectId = _context.Projects.Max(p => p.ProjectId) + 1;

                //        _context.Projects.Add(pjname);
                //    }
                //}

                ////最後搭橋梁
                ////int pid 為 Plan資料表中最後一個欄位的ID
                //var pid = _context.Plans.Max(p => p.PlanId);

                //foreach (var item in pjid)
                //{
                //    var a = new PlanRef();
                //    a.PlanId = pid;
                //    a.ProjectId = Int32.Parse(item);

                //    _context.PlanRefs.Add(a);
                //}
                //-----------------------------------------------------

                _context.SaveChanges();

                //dbContextTransaction.Commit();

                return Ok();
            }

            catch (Exception ex)
            {
                //dbContextTransaction.Rollback();
                return BadRequest(ex.Message);
                
            }
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
