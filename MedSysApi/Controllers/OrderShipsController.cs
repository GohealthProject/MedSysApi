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
    public class OrderShipsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public OrderShipsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/OrderShips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderShip>>> GetOrderShips()
        {
          if (_context.OrderShips == null)
          {
              return NotFound();
          }
            return await _context.OrderShips.ToListAsync();
        }

        // GET: api/OrderShips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderShip>> GetOrderShip(int id)
        {
          if (_context.OrderShips == null)
          {
              return NotFound();
          }
            var orderShip = await _context.OrderShips.FindAsync(id);

            if (orderShip == null)
            {
                return NotFound();
            }

            return orderShip;
        }

        // PUT: api/OrderShips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderShip(int id, OrderShip orderShip)
        {
            if (id != orderShip.ShipId)
            {
                return BadRequest();
            }

            _context.Entry(orderShip).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderShipExists(id))
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

        // POST: api/OrderShips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderShip>> PostOrderShip(OrderShip orderShip)
        {
          if (_context.OrderShips == null)
          {
              return Problem("Entity set 'MedSysContext.OrderShips'  is null.");
          }
            _context.OrderShips.Add(orderShip);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderShip", new { id = orderShip.ShipId }, orderShip);
        }
        [HttpPost("arrayByship")]
        public IActionResult arrayByship([FromBody] int[] nums)
        {
            List<string> list = new List<string>();
            foreach (int i in nums)
            {
                var q = _context.OrderShips.Find(i);
                list.Add(q.ShipName);
            }

            return Ok(list);
        }
        // DELETE: api/OrderShips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderShip(int id)
        {
            if (_context.OrderShips == null)
            {
                return NotFound();
            }
            var orderShip = await _context.OrderShips.FindAsync(id);
            if (orderShip == null)
            {
                return NotFound();
            }

            _context.OrderShips.Remove(orderShip);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderShipExists(int id)
        {
            return (_context.OrderShips?.Any(e => e.ShipId == id)).GetValueOrDefault();
        }
    }
}
