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
    public class ProductsCategoriesController : ControllerBase
    {
        private readonly MedSysContext _context;

        public ProductsCategoriesController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/ProductsCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsCategory>>> GetProductsCategories()
        {
          if (_context.ProductsCategories == null)
          {
              return NotFound();
          }
            return await _context.ProductsCategories.ToListAsync();
        }

        // GET: api/ProductsCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsCategory>> GetProductsCategory(int id)
        {
          if (_context.ProductsCategories == null)
          {
              return NotFound();
          }
            var productsCategory = _context.ProductsCategories.Find(id);
            if (productsCategory == null)
            {
                return NotFound();
            }

            return productsCategory;
        }

        // PUT: api/ProductsCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductsCategory(int id, ProductsCategory productsCategory)
        {
            if (id != productsCategory.CategoriesId)
            {
                return BadRequest();
            }

            _context.Entry(productsCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsCategoryExists(id))
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

        // POST: api/ProductsCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductsCategory>> PostProductsCategory(ProductsCategory productsCategory)
        {
          if (_context.ProductsCategories == null)
          {
              return Problem("Entity set 'MedSysContext.ProductsCategories'  is null.");
          }
            _context.ProductsCategories.Add(productsCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductsCategory", new { id = productsCategory.CategoriesId }, productsCategory);
        }

        // DELETE: api/ProductsCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductsCategory(int id)
        {
            if (_context.ProductsCategories == null)
            {
                return NotFound();
            }
            var productsCategory = await _context.ProductsCategories.FindAsync(id);
            if (productsCategory == null)
            {
                return NotFound();
            }

            _context.ProductsCategories.Remove(productsCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsCategoryExists(int id)
        {
            return (_context.ProductsCategories?.Any(e => e.CategoriesId == id)).GetValueOrDefault();
        }
    }
}
