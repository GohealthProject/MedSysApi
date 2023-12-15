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
    public class TrackingListsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public TrackingListsController(MedSysContext context)
        {
            _context = context;
        }
        [HttpGet("Delete/{Mid}&{Tid}")]
        public IActionResult DeleteTracking(int Mid,int Tid)
        {
            var TrackingList = _context.TrackingLists.Where(x => x.MemberId == Mid && x.TrackingListId == Tid).FirstOrDefault();

            _context.TrackingLists.Remove(TrackingList);
            _context.SaveChanges();

            var ReTrackingList = _context.TrackingLists.Include(n=>n.Product).Where(x => x.MemberId == Mid).ToList();


            return Ok(ReTrackingList);
        }
        // GET: api/TrackingLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackingList>>> GetTrackingLists()
        {
          if (_context.TrackingLists == null)
          {
              return NotFound();
          }
            return await _context.TrackingLists.ToListAsync();
        }

        // GET: api/TrackingLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackingList>> GetTrackingList(int id)
        {
          if (_context.TrackingLists == null)
          {
              return NotFound();
          }
            var trackingList = await _context.TrackingLists.FindAsync(id);

            if (trackingList == null)
            {
                return NotFound();
            }

            return trackingList;
        }

        // PUT: api/TrackingLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrackingList(int id, TrackingList trackingList)
        {
            if (id != trackingList.TrackingListId)
            {
                return BadRequest();
            }

            _context.Entry(trackingList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackingListExists(id))
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

        // POST: api/TrackingLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackingList>> PostTrackingList(TrackingList trackingList)
        {
          if (_context.TrackingLists == null)
          {
              return Problem("Entity set 'MedSysContext.TrackingLists'  is null.");
          }
            _context.TrackingLists.Add(trackingList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrackingList", new { id = trackingList.TrackingListId }, trackingList);
        }

        // DELETE: api/TrackingLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackingList(int id)
        {
            if (_context.TrackingLists == null)
            {
                return NotFound();
            }
            var trackingList = await _context.TrackingLists.FindAsync(id);
            if (trackingList == null)
            {
                return NotFound();
            }

            _context.TrackingLists.Remove(trackingList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackingListExists(int id)
        {
            return (_context.TrackingLists?.Any(e => e.TrackingListId == id)).GetValueOrDefault();
        }
    }
}
