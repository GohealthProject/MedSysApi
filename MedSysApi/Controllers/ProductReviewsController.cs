using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using Newtonsoft.Json;

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
        [HttpGet("changeLike/{key}&{Reviewid}&{memberid}")]
        public IActionResult changeLike(string key,int Reviewid,int memberid)
        {
            var q = _context.ProductReviews.Where(n => n.ProductReviewId == Reviewid).Include(n => n.Product).FirstOrDefault();

            if(q.MemberId!= memberid)
                return BadRequest();

            if(key == "good.png")
            {
                q.IsLike = false;
                q.Product.Likecount--;
            }
            else
            {
                q.IsLike = true;
                q.Product.Likecount++;
            }
            _context.SaveChanges();
            return Ok();
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
            var productReviews = _context.ProductReviews.Where(p => p.ProductId == productID).Include(n => n.Member).OrderByDescending(n=>n.Timestamp);
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(productReviews, setting);
            return Ok(json);
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
            Product p =_context.Products.Find(productID) ;
            p.Likecount++;
            _context.SaveChanges();

            var productReviews = _context.ProductReviews.Where(p => p.ProductId == productID).Include(n => n.Member).OrderByDescending(n => n.ProductReviewId).FirstOrDefault();

            return Ok(productReviews);
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
