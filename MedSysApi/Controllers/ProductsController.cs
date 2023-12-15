using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using Microsoft.OpenApi.Validations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public ProductsController(MedSysContext context)
        {
            _context = context;
        }
        [HttpGet("Suggested/{Pid}")]
        public IActionResult Suggested(int Pid)
        {
            var PidFindCategory = _context.Products.Where(n => n.ProductId == Pid).Select(n => n.ProductsClassifications).SelectMany(n => n); //找到該產品的所有類別
            List<int> CategoryIDlist = new List<int>();
            foreach(var item in PidFindCategory)
            {
                CategoryIDlist.Add(item.CategoriesId);
            }
            var CategoryFindProduct = _context.ProductsClassifications.Where(n => CategoryIDlist.Contains(n.CategoriesId)).Select(n => n.ProductId); //找到類別的所有產品ID
            List<int>list2 = new List<int>();
            list2= CategoryFindProduct.ToList();

            var CategoryHighLight = _context.OrderDetails.Where(n => list2.Contains((int)n.ProductId)).GroupBy(n => n.ProductId).Select(n => new
            {
                key = n.Key,
                count = n.Sum(n => n.Quantity)
            }).OrderByDescending(n=>n.count); //找到該類別的銷量最高的產品
            List<int> list3 = new List<int>();
            foreach(var item in CategoryHighLight)
            {
                list3.Add((int)item.key);
            }
            var q = _context.Products.Where(n => list3.Contains(n.ProductId)).Take(6);
            List<Product> proList = new List<Product>();
            proList = q.ToList();
            return Ok(proList);
        }
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }
        [HttpGet("{key}&{page}")]
        public IActionResult getProductPage(string key , int page)
        {
            if (_context.Products == null)
            {
                return BadRequest();
            }
            
            int pagecount = 9 * page;
            var q = _context.Products.Where(n => n.ProductName.Contains(key)).ToList();

            if (page == 1) 
                return Ok(q.Take(9));
            else
            {
                var g = q.Take(9*page).Skip((page-1)*9);
                return Ok(g);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Content("hello");
        }
        
        // GET: api/Products/5
        [HttpGet("category/{categoryID}")]
        public async Task<ActionResult<Product>> CategoryIDGetProduct(int categoryID)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = _context.ProductsCategories.Where(n => n.CategoriesId == categoryID)
                .Include(n => n.ProductsClassifications)
                .ThenInclude(n => n.Product);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'MedSysContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("hot/key={keyword}")]
        public IActionResult hot(string keyword)
        {
            var q = _context.OrderDetails
                .Where(od => od.Product.ProductName
                .Contains(keyword))
                .GroupBy(od => od.ProductId)
                .Select(n => new
            {
                pid = n.Key,
                qua = n.Sum(n => n.Quantity)
            }).OrderBy(n => n.qua).ToList(); ;
          
            List<Product> list = new List<Product>();
            foreach(var item in q)
            {
                var pro = _context.Products.Where(n => n.ProductId == item.pid).FirstOrDefault();
                list.Add((Product)pro);
            }


            return Ok(list);
        }
        [HttpGet("top/key={keyword}")]
        public IActionResult top5(string keyword)
        {
            return Ok();
        }
        [HttpGet("news/key={keyword}")]
        public IActionResult news(string keyword)
        {
            var q = _context.Products.Where(n => n.ProductName.Contains(keyword)).OrderByDescending(n => n.ProductId);
            List<Product> list = new List<Product>();
            foreach(var item in q)
            {
                list.Add(item);
            }   
            return Ok(list);
        }
        [HttpGet("sort/{keyword}&{sort}")]
        public IActionResult sort(string keyword , string sort)
        {
            List<Product> list = new List<Product>();
            if(sort == "Up")
            {
                var q = _context.Products.Where(n=>n.ProductName.Contains(keyword)).OrderBy(n => n.UnitPrice);
                foreach (var item in q)
                {
                    list.Add(item);
                }
                return Ok(list);
            }
            else if(sort=="low")
            {
                var q = _context.Products.Where(n=>n.ProductName.Contains(keyword)).OrderByDescending(n => n.UnitPrice);
                foreach (var item in q)
                {
                    list.Add(item);
                }
                return Ok(list);
            }
            else
            {
                return BadRequest();
            }

        }
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
