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
    public class MembersController : ControllerBase
    {
        private readonly MedSysContext _context;

        public MembersController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            return await _context.Members.ToListAsync();
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<Member>> GetMember(string name)
        {
            if (_context.Members == null)
                return NotFound();

            Member? member = await _context.Members.FirstOrDefaultAsync(n => n.MemberName == name|| n.MemberEmail ==name||n.MemberPhone==name);
            if (member == null)
                return NotFound();

            return member;
        }
       
        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
          if (_context.Members == null)
          {
              return NotFound();
          }
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Up/{id}")]
        public IActionResult PutMem(int id)
        {

            //byte[] img = null;

            //var file = Request.Form.Files;
            //using (var memoryStream = new MemoryStream())
            //{
            //    file[0].CopyTo(memoryStream);
            //    img = memoryStream.ToArray();
            //}

            var mem = Request.Form;
            int mid = Int32.Parse(mem["MemberId"]);
            string name = mem["MemberName"];
            string gender = mem["MemberGender"];
            string phone = mem["MemberPhone"];
            string email = mem["MemberEmail"];
            string address = mem["MemberAddress"];
            string contact = mem["MemberContact"];
            string nick = mem["MemberNick"];
            string pwd = mem["MemberPassWord"];

            var upmem = _context.Members.Where(n => n.MemberId == id).FirstOrDefault();
            upmem.MemberName = name;
            upmem.MemberGender = gender;
            upmem.MemberPhone = phone;
            upmem.MemberEmail = email;
            upmem.MemberAddress = address;
            upmem.MemberContactNumber = contact;
            upmem.MemberNickname = nick;
            upmem.MemberPassword = pwd;
            _context.SaveChanges();

            return Ok();
        }

            // PUT: api/Members/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int id, Member member)
        {
            var mem = Request.Form;
            int mid = Int32.Parse(mem["MemberId"]);
            string name = mem["MemberName"];
            string gender = mem["MemberGender"];
            string phone = mem["MemberPhone"];
            string email = mem["MemberEmail"];
            string address = mem["MemberAddress"];
            string contact = mem["MemberContact"];
            string nick = mem["MemberNick"];
            string pwd = mem["MemberPassWord"];

            var upmem = _context.Members.Where(n => n.MemberId == id).FirstOrDefault();
            upmem.MemberName = name;
            upmem.MemberGender = gender;
            upmem.MemberPhone = phone;
            upmem.MemberEmail = email;
            upmem.MemberAddress = address;
            upmem.MemberContactNumber = contact;
            upmem.MemberNickname = nick;
            upmem.MemberPassword = pwd;
            _context.SaveChanges();

            if (id != member.MemberId)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
          if (_context.Members == null)
          {
              return Problem("Entity set 'MedSysContext.Members'  is null.");
          }
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.MemberId }, member);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
