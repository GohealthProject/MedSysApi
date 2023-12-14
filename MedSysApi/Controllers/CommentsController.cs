using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using System.Text.Json;
using Humanizer;

namespace MedSysApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly MedSysContext _context;

        public CommentsController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
          if (_context.Comments == null)
          {
              return NotFound();
          }
            return await _context.Comments.ToListAsync();
        }
        [HttpGet("comment")]
        public IActionResult comment(int id)
        {
            var q = _context.Comments.Where(n => n.CommentId == id).ToList();

            var qq = _context.Comments.Where(n=>n.ParentCommentId==id).Include(n=>n.Member).Include(n=>n.Employee);
            
            return Ok(qq);
        }
        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
          if (_context.Comments == null)
          {
              return NotFound();
          }
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
          if (_context.Comments == null)
          {
              return Problem("Entity set 'MedSysContext.Comments'  is null.");
          }
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        [HttpPost("memberAddComment")]
        public async Task<ActionResult> memberAddComment([FromBody] Comment comment) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Comment newComment = new Comment
                    {
                        BlogId = comment.BlogId,
                        MemberId = comment.MemberId,
                        EmployeeId = null,
                        ParentCommentId = null,
                        Content = comment.Content,
                        CreatedAt = DateTime.Now,
                    };

                    _context.Comments.Add(newComment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Comment saved successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"An error occured:{ex.Message}");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
