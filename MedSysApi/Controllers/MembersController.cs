using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using System.Net;
using static System.Net.WebRequestMethods;

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
            string gender = mem["Gender"];
            string phone = mem["MemberPhoneNum"];
            string birthDate = mem["MemberBirthDate"];
            string email = mem["MemberEmail"];
            string address = mem["MemberAddress"];
            string contact = mem["MemberContact"];
            string nick = mem["MemberNick"];
            string pwd = mem["MemberPassWord"];

            var file = Request.Form.Files;

            var upmem = _context.Members.Where(n => n.MemberId == id).FirstOrDefault();
            upmem.MemberName = name;
            upmem.MemberGender = gender;
            DateTime birthdate = DateTime.Parse(birthDate);
            upmem.MemberBirthdate = birthdate;
            upmem.MemberPhone = phone;
            upmem.MemberEmail = email;
            upmem.MemberAddress = address;
            upmem.MemberContactNumber = contact;
            upmem.MemberNickname = nick;
            upmem.MemberPassword = pwd;
            string formimg = file[0].FileName;
            upmem.MemberImage = file[0].FileName;
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
            string birth = mem["MemberBirthDate"];
            string phone = mem["MemberPhone"];
            string email = mem["MemberEmail"];
            string address = mem["MemberAddress"];
            string contact = mem["MemberContact"];
            string nick = mem["MemberNick"];
            string pwd = mem["MemberPassWord"];

            var upmem = _context.Members.Where(n => n.MemberId == id).FirstOrDefault();
            upmem.MemberName = name;
            upmem.MemberGender = gender;
            DateTime birthdate = DateTime.Parse(birth);
            upmem.MemberBirthdate = birthdate;
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
        public IActionResult DeleteMember(int id)
        {
            ModelBuilder modelBuilder = new ModelBuilder();

            

            if (_context.Members == null)
            {
                return NotFound();
            }
            var member = _context.Members.Find(id);//找到會員

            var od = _context.Orders.Where(n => n.MemberId == id);//找到會員的所有訂單
            var hp = _context.HealthReports.Where(n => n.MemberId == id);//找到會員的所有健康報告
            var rev = _context.Reserves.Where(n => n.MemberId == id);//找到會員所有的預約
            List<int> revID = new List<int>(); //找到所有健康報告的ID 
            List<int> resSubID = new List<int>(); //找到所有預約的ID
            foreach (var item in hp)//找所有健康報告的ID
            {
                revID.Add(item.ReportId);
            }
            

            var revdet = _context.ReportDetails.Where(n => revID.Contains((int)n.ReportId)); //用找到的健康報告ID去刪除所有該ID的詳細項目
            foreach(var item in revdet) //刪掉所有健康報告詳細
            {
                _context.ReportDetails.Remove(item);
            } //把該會員所有健康報告項目ID刪掉
            foreach(var item in hp)//刪掉所有健康報告
            {
                _context.HealthReports.Remove(item);
            } //把該會員所有健康報告刪掉

            foreach(var item in rev) //找到所有預約的ID
            {
                resSubID.Add(item.ReserveId);
            }
            var resSub = _context.ReservedSubs.Where(n => resSubID.Contains(n.ReservedId)); //找到所有預約SUB的ID
            foreach(var item in resSub) //把所有預約SUB刪掉
            {
                _context.ReservedSubs.Remove(item);
            }

            foreach(var item in rev) //把所有預約刪掉
            {
                _context.Reserves.Remove(item);
            }
            List<int> oid = new List<int>();

            foreach(var item in od) //找到該會員所有訂單的ID
            {
                int i = item.OrderId;
                oid.Add(i);
            } 
            var orderDe = _context.OrderDetails.Where(n => oid.Contains((int)n.OrderId)); //找到所有訂單詳細的ID
            foreach(var item in orderDe)
            {
                _context.OrderDetails.Remove(item);
            } //刪掉該會員所有訂單詳細

            foreach(var o in od)
            {
                _context.Orders.Remove(o);
            } //刪掉該會員所有訂單
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member); //在刪掉該會員
            _context.SaveChanges();

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return (_context.Members?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
