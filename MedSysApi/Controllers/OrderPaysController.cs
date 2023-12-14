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
    public class OrderPaysController : ControllerBase
    {
        private readonly MedSysContext _context;

        public OrderPaysController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/OrderPays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderPay>>> GetOrderPays()
        {
            if (_context.OrderPays == null)
            {
                return NotFound();
            }
            return await _context.OrderPays.ToListAsync();
        }

        // GET: api/OrderPays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderPay>> GetOrderPay(int id)
        {
            if (_context.OrderPays == null)
            {
                return NotFound();
            }
            var orderPay = await _context.OrderPays.FindAsync(id);

            if (orderPay == null)
            {
                return NotFound();
            }

            return orderPay;
        }

        // PUT: api/OrderPays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderPay(int id, OrderPay orderPay)
        {
            if (id != orderPay.PayId)
            {
                return BadRequest();
            }

            _context.Entry(orderPay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderPayExists(id))
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

        // POST: api/OrderPays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderPay>> PostOrderPay(OrderPay orderPay)
        {
            if (_context.OrderPays == null)
            {
                return Problem("Entity set 'MedSysContext.OrderPays'  is null.");
            }
            _context.OrderPays.Add(orderPay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderPay", new { id = orderPay.PayId }, orderPay);
        }
        [HttpPost("arrayBypay")]
        public IActionResult arrayBypay([FromBody] int[] nums)
        {
            List<string> list = new List<string>();
            foreach(int i in nums)
            {
                var q = _context.OrderPays.Find(i);
                list.Add(q.PayName);
            }

            return Ok(list);
        }
        // DELETE: api/OrderPays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderPay(int id)
        {
            if (_context.OrderPays == null)
            {
                return NotFound();
            }
            var orderPay = await _context.OrderPays.FindAsync(id);
            if (orderPay == null)
            {
                return NotFound();
            }

            _context.OrderPays.Remove(orderPay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderPayExists(int id)
        {
            return (_context.OrderPays?.Any(e => e.PayId == id)).GetValueOrDefault();
        }
    }
}
