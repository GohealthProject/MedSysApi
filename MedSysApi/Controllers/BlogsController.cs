using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinifyAPI;
using MedSysApi.Models;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public BlogsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            return await _context.Blogs.ToListAsync();
        }
        [HttpGet("popular6")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetPopular6Blogs()
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blogs = await _context.Blogs.Include(blog => blog.ArticleClass).Include(blog => blog.Employee).OrderByDescending(blog => blog.Views).Take(6).ToListAsync();

            var infoIonlyWant = blogs.Select(blog => new
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Author = blog.Employee.EmployeeName,
                ArticleClass = blog.ArticleClass.BlogCategory1,
                CreatedAt = blog.CreatedAt,
                Views =blog.Views
            });
            return Ok(infoIonlyWant);
        }   


        [HttpGet("latest6")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetLatest6Blogs()
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blogs = await _context.Blogs.Include(blog => blog.ArticleClass).Include(blog => blog.Employee).OrderByDescending(blog => blog.BlogId).Take(6).ToListAsync();

            var infoIonlyWant = blogs.Select(blog => new
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Author = blog.Employee.EmployeeName,
                ArticleClass = blog.ArticleClass.BlogCategory1,
                CreatedAt = blog.CreatedAt,
                Views =blog.Views
            });
            return Ok(infoIonlyWant);
        }

        [HttpGet("thisCat6")]
        public async Task<ActionResult<IEnumerable<Blog>>>GetThisCat6Blogs(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blogs = await _context.Blogs.Include(blog => blog.ArticleClass).Include(blog => blog.Employee).Where(blog => blog.ArticleClassId == id).OrderByDescending(blog => blog.BlogId).Take(6).ToListAsync();

            var infoIonlyWant = blogs.Select(blog => new
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Author = blog.Employee.EmployeeName,
                ArticleClass = blog.ArticleClass.BlogCategory1,
                CreatedAt = blog.CreatedAt,
                Views =blog.Views
            });
            return Ok(infoIonlyWant);
        }
        [HttpGet("activity6")]
        public async Task<ActionResult<IEnumerable<Blog>>>GetActivity6Blogs()
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blogs = await _context.Blogs.Include(blog => blog.ArticleClass).Include(blog => blog.Employee).Where(blog => blog.ArticleClassId == 1).OrderByDescending(blog => blog.Views).Take(6).ToListAsync();

            var infoIonlyWant = blogs.Select(blog => new
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Author = blog.Employee.EmployeeName,
                ArticleClass = blog.ArticleClass.BlogCategory1,
                CreatedAt = blog.CreatedAt,
                Views =blog.Views
            });
            return Ok(infoIonlyWant);
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }
        [HttpGet("detail")]
        public async Task<ActionResult<Blog>> GetBlogDetail(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.Include(blog => blog.ArticleClass).Include(blog => blog.Employee).FirstOrDefaultAsync(blog => blog.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }
            blog.Views++;
            await _context.SaveChangesAsync();
            var infoIonlyWant = new
            {
                BlogId = blog.BlogId,
                Title = blog.Title,
                Author = blog.Employee.EmployeeName,
                ArticleClass = blog.ArticleClass.BlogCategory1,
                CreatedAt = blog.CreatedAt,
                Views =blog.Views,
                Content = blog.Content,
                BlogImage = blog.BlogImage
            };
            return Ok(infoIonlyWant);
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBlog(int id)
        {
            try
            {
                var blog = await _context.Blogs.FindAsync(id);
                Tinify.Key = "rhkRy28T0Xz4JDdQ1y45cbxNTW47Gm46";//
                if (blog == null)
                {
                    return NotFound();
                }
                //獲取put請求的資料
                var blogData = Request.Form;
                string title = blogData["Title"];
                int articleClassId = int.Parse(blogData["ArticleClassId"]);
                string content = blogData["Content"];
                int employeeId = int.Parse(blogData["EmployeeId"]);
                var files = Request.Form.Files;

                blog.Title = title;
                blog.ArticleClassId = articleClassId;
                blog.Content = content;
                blog.EmployeeId = employeeId;
                if (files.Any())
                {
                    if (files[0] != null && files[0].Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            files[0].CopyTo(ms);
                            var source = Tinify.FromBuffer(ms.ToArray());
                            var resize = await source.ToBuffer();
                            blog.BlogImage = resize;
                        }
                    }
                }


                _context.Entry(blog).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured:{ex.Message}");
            }
            return Ok();
        }

        // POST: api/Blogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog()
        {
            try
            {
                byte[] img = null;
                var blog = Request.Form;
                Tinify.Key = "rhkRy28T0Xz4JDdQ1y45cbxNTW47Gm46";
                string title = blog["Title"];
                int ArticleClassId = int.Parse(blog["ArticleClassId"]);
                string content = blog["Content"];
                var file = Request.Form.Files;//檔案
                if (file.Any())
                {
                    if (file[0] != null && file[0].Length > 0)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            file[0].CopyTo(memoryStream);
                            var source = Tinify.FromBuffer(memoryStream.ToArray());
                            var resize = await source.ToBuffer();
                            img = resize;
                        }
                    }
                }
                int employeeId = int.Parse(blog["EmployeeId"]);
                Blog newBlog = new Blog();
                newBlog.Title = title;
                newBlog.ArticleClassId = ArticleClassId;
                newBlog.Content = content;
                newBlog.EmployeeId = employeeId;
                newBlog.BlogImage = img;
                newBlog.Views = 0;
                newBlog.CreatedAt = DateTime.Now;
                _context.Blogs.Add(newBlog);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured:{ex.Message}");
            }
            return Ok();

        }

        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}
