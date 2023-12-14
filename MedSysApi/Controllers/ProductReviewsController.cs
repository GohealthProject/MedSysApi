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
    public class ProductReviewsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public ProductReviewsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/ProductReviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReview>>> GetProductReviews()
        {
          if (_context.ProductReviews == null)
          {
              return NotFound();
          }
            return await _context.ProductReviews.ToListAsync();
        }
        [HttpGet("pid={productID}")]
        public IActionResult GetProductReviews(int productID)
        {
          if (_context.ProductReviews == null)
            {
              return NotFound();
          }
            var productReviews = _context.ProductReviews.Where(p => p.ProductId == productID).Include(n => n.Member);
            return Ok(productReviews);
        }
        // GET: api/ProductReviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReview>> GetProductReview(int id)
        {
          if (_context.ProductReviews == null)
          {
              return NotFound();
          }
            var productReview = await _context.ProductReviews.FindAsync(id);

            if (productReview == null)
            {
                return NotFound();
            }

            return productReview;
        }

        // PUT: api/ProductReviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductReview(int id, ProductReview productReview)
        {
            if (id != productReview.ProductReviewId)
            {
                return BadRequest();
            }

            _context.Entry(productReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductReviewExists(id))
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

        // POST: api/ProductReviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("mid={memberID}&pid={productID}")]
        public IActionResult PostProductReview( int memberID,int productID)
        {
            var q = Request.Form;
            var review = q["review"];
            ProductReview productReview = new ProductReview();
            productReview.MemberId = memberID;
            productReview.ProductId = productID;
            productReview.ReviewContent = review;
            productReview.Timestamp = DateTime.Now;
            productReview.IsLike = true;
            _context.ProductReviews.Add(productReview); 
            _context.SaveChanges();
            return Content("留言成功");
        }
        [HttpPost]
        public async Task<ActionResult<ProductReview>> PostProductReview(ProductReview productReview)
        {
          if (_context.ProductReviews == null)
          {
              return Problem("Entity set 'MedSysContext.ProductReviews'  is null.");
          }
            _context.ProductReviews.Add(productReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductReview", new { id = productReview.ProductReviewId }, productReview);
        }

        // DELETE: api/ProductReviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductReview(int id)
        {
            if (_context.ProductReviews == null)
            {
                return NotFound();
            }
            var productReview = await _context.ProductReviews.FindAsync(id);
            if (productReview == null)
            {
                return NotFound();
            }

            _context.ProductReviews.Remove(productReview);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductReviewExists(int id)
        {
            return (_context.ProductReviews?.Any(e => e.ProductReviewId == id)).GetValueOrDefault();
        }
    }
}
