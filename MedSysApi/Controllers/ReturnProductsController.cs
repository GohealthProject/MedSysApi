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
    public class ReturnProductsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public ReturnProductsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/ReturnProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnProduct>>> GetReturnProducts()
        {
          if (_context.ReturnProducts == null)
          {
              return NotFound();
          }
            return await _context.ReturnProducts.ToListAsync();
        }

        // GET: api/ReturnProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnProduct>> GetReturnProduct(int id)
        {
          if (_context.ReturnProducts == null)
          {
              return NotFound();
          }
            var returnProduct = await _context.ReturnProducts.FindAsync(id);

            if (returnProduct == null)
            {
                return NotFound();
            }

            return returnProduct;
        }

        // PUT: api/ReturnProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public IActionResult Create()
        {
            var form = Request.Form;
            var orderid = form["orderid"];
            var reason = form["ReturnReason"];
            var orderAmount = form["orderAmount"];
            ReturnProduct re = new ReturnProduct();
            re.ReturnDate = DateTime.Now;
            re.OrderId = int.Parse(orderid);
            re.ReturnReason = reason;
            re.RefundAmount = decimal.Parse(orderAmount);
            re.ReturnState = "待處理";
            _context.ReturnProducts.Add(re);
            
            var order = _context.Orders.Find(int.Parse(orderid));
            order.StateId = 15;
            _context.SaveChanges();

            
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReturnProduct(int id, ReturnProduct returnProduct)
        {
            if (id != returnProduct.ReturnId)
            {
                return BadRequest();
            }

            _context.Entry(returnProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnProductExists(id))
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

        // POST: api/ReturnProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReturnProduct>> PostReturnProduct(ReturnProduct returnProduct)
        {
          if (_context.ReturnProducts == null)
          {
              return Problem("Entity set 'MedSysContext.ReturnProducts'  is null.");
          }
            _context.ReturnProducts.Add(returnProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReturnProduct", new { id = returnProduct.ReturnId }, returnProduct);
        }

        // DELETE: api/ReturnProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReturnProduct(int id)
        {
            if (_context.ReturnProducts == null)
            {
                return NotFound();
            }
            var returnProduct = await _context.ReturnProducts.FindAsync(id);
            if (returnProduct == null)
            {
                return NotFound();
            }

            _context.ReturnProducts.Remove(returnProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReturnProductExists(int id)
        {
            return (_context.ReturnProducts?.Any(e => e.ReturnId == id)).GetValueOrDefault();
        }
    }
}
