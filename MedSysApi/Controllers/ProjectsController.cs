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
    public class ProjectsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public ProjectsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("prj/{id}")]
        public IActionResult PProject(int id)
        {
            var q = Request.Form;
            var ProjectName = q["ProjectName"];
            var ProjectPrice = q["ProjectPrice"];

            var project = _context.Projects.Find(id);
            project.ProjectName = ProjectName;
            project.ProjectPrice = Convert.ToInt32(ProjectPrice);

            _context.SaveChanges();
            return Ok(project);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostProject()
        {
            var q = Request.Form;
            var Cprjtxt = q["Cprjtxt"]; //陣列
            var prjtxt = q["prjtxt"]; //陣列

            if (Cprjtxt.Any())
            {
                foreach (var item in Cprjtxt)
                {
                    if (item != null)
                    {
                        var pjname = new Project();
                        pjname.ProjectName = item;
                        pjname.ProjectPrice = 0;

                        pjname.ProjectId = _context.Projects.Max(p => p.ProjectId) + 1;

                        _context.Projects.Add(pjname);
                        _context.SaveChanges();
                    }
                }
            }
            else if (prjtxt.Any())
            {
                foreach (var item in prjtxt)
                {
                    if (item != null)
                    {
                        var pjname = new Project
                        {
                            ProjectName = item,
                            ProjectPrice = 0,
                            ProjectId = _context.Projects.Max(p => p.ProjectId) + 1
                    };

                        _context.Projects.Add(pjname);
                        _context.SaveChanges();
                    }
                }
            }

            
            //string pjname = q["prjchk"];



            return Ok();

            //if (_context.Projects == null)
            //{
            //    return Problem("Entity set 'MedSysContext.Projects'  is null.");
            //}
            //  _context.Projects.Add(project);
            //  try
            //  {
            //      await _context.SaveChangesAsync();
            //  }
            //  catch (DbUpdateException)
            //  {
            //      if (ProjectExists(project.ProjectId))
            //      {
            //          return Conflict();
            //      }
            //      else
            //      {
            //          throw;
            //      }
            //  }

            //  return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        [HttpPost("prj")]
        public IActionResult PostPrj()
        {
            var q = Request.Form;
            var CProjectName = q["CProjectName"];
            var CProjectPrice = q["CProjectPrice"];

            var project  = new Project
            {
                ProjectName = CProjectName,
                ProjectPrice = Convert.ToInt32(CProjectPrice),
                ProjectId = _context.Projects.Max(p => p.ProjectId) + 1
            };
            _context.Projects.Add(project);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            //先刪除PlanRefs中的資料
            var q = _context.PlanRefs.Where(p => p.ProjectId == id);
            foreach (var item in q)
            {
                _context.PlanRefs.Remove(item);
            }

            //再刪除Projects中的資料
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
