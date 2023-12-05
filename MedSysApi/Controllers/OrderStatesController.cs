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
    public class OrderStatesController : ControllerBase
    {
        private readonly MedSysContext _context;

        public OrderStatesController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/OrderStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderState>>> GetOrderStates()
        {
          if (_context.OrderStates == null)
          {
              return NotFound();
          }
            return await _context.OrderStates.ToListAsync();
        }

        // GET: api/OrderStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderState>> GetOrderState(int id)
        {
          if (_context.OrderStates == null)
          {
              return NotFound();
          }
            var orderState = await _context.OrderStates.FindAsync(id);

            if (orderState == null)
            {
                return NotFound();
            }

            return orderState;
        }

        // PUT: api/OrderStates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderState(int id, OrderState orderState)
        {
            if (id != orderState.StateId)
            {
                return BadRequest();
            }

            _context.Entry(orderState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderStateExists(id))
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

        // POST: api/OrderStates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderState>> PostOrderState(OrderState orderState)
        {
          if (_context.OrderStates == null)
          {
              return Problem("Entity set 'MedSysContext.OrderStates'  is null.");
          }
            _context.OrderStates.Add(orderState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderState", new { id = orderState.StateId }, orderState);
        }
        [HttpPost("arrayBystate")]
        public IActionResult arrayBystate([FromBody] int[] nums)
        {
            List<string> list = new List<string>();
            foreach (int i in nums)
            {
                var q = _context.OrderStates.Find(i);
                list.Add(q.StateName);
            }

            return Ok(list);
        }

        // DELETE: api/OrderStates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderState(int id)
        {
            if (_context.OrderStates == null)
            {
                return NotFound();
            }
            var orderState = await _context.OrderStates.FindAsync(id);
            if (orderState == null)
            {
                return NotFound();
            }

            _context.OrderStates.Remove(orderState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderStateExists(int id)
        {
            return (_context.OrderStates?.Any(e => e.StateId == id)).GetValueOrDefault();
        }
    }
}
