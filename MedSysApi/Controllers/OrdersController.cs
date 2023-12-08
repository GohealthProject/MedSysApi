using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSysApi.Models;
using System.Buffers;
using MedSysProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MedSysApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MedSysContext _context;

        public OrdersController(MedSysContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'MedSysContext.Orders'  is null.");
          }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        [HttpGet("Testt/{id}")]
        public IActionResult ReadOrderTest(string? key, int? page, int id)
        {

            List<COrderWarp> list = new List<COrderWarp>();
            //string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
            //MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
            var qq = key;

            if (qq == null)
            {

                var midFindOrder = _context.Orders.Where(n => n.MemberId == id)
                    .Include(n => n.Pay)
                    .Include(n => n.Ship)
                    .Include(n => n.State)
                    .Include(n => n.OrderDetails)
                    .ThenInclude(n => n.Product)
                    .OrderByDescending(n => n.OrderDate)
                    .ToList();

                //var midFindOrder = _context.Orders.Where(n => n.MemberId == id).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate);
                foreach (var item in midFindOrder)
                {
                    COrderWarp o = new COrderWarp();
                    o.order = item;
                    list.Add(o);
                }

                return Ok(list);
            }
            return Ok();
        }

        [HttpGet("Read/{id}")]
        public IActionResult ReadOrder(string? key,string? datekey,int? page,int id)
        {
            var pp = page;
            //page = pp == "" ? 1 : int.Parse(pp);

            if (key != "")
            {
                List<COrderWarp> list = new List<COrderWarp>();
                //string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                //MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
                var qq = key;

                if (qq == "")
                {
                    //分頁
                    int pageSize = 5;
                    int total = _context.Orders.Where(n => n.MemberId == id).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate).Skip(((int)page - 1) * pageSize).Take(pageSize).Count();
                    int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    if (page > totalPage)
                        page = totalPage;
                    if (page < 1)
                        page = 1;

                    var midFindOrder = _context.Orders.Where(n => n.MemberId == id).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate).Skip(((int)page - 1) * pageSize).Take(pageSize);

                    //var midFindOrder = _context.Orders.Where(n => n.MemberId == id).Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).OrderByDescending(n => n.OrderDate);
                    foreach (var item in midFindOrder)
                    {
                        COrderWarp o = new COrderWarp();
                        o.order = item;
                        list.Add(o);
                    }
                    return Ok(list);
                }
                else
                {
                    string? keyword = key;
                    List<int> pids = new List<int>();
                    List<int> oids = new List<int>();

                    pids = _context.Products.Where(n => n.ProductName.Contains(keyword)).Select(n => n.ProductId).ToList();
                    oids = _context.Members.Where(n => n.MemberId == id).Include(n => n.Orders).ThenInclude(n => n.OrderDetails).SelectMany(n => n.Orders.Where(n => n.OrderDetails.Any(n => pids.Contains((int)n.ProductId))).Select(n => n.OrderId)).ToList();

                    //分頁
                    int pageSize = 5;
                    int total = _context.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId)).Count();
                    int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    if (page > totalPage)
                        page = totalPage;
                    if (page < 1)
                        page = 1;

                    var q = _context.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).Skip(((int)page - 1) * pageSize).Take(pageSize);

                    //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => oids.Contains(n.OrderId));
                    foreach (var o in q)
                    {
                        COrderWarp od = new COrderWarp();
                        od.order = o;
                        list.Add(od);
                    }
                    return Ok(list);
                }
            }
            else if (datekey != "")
            {
                var form = Request.Form;
                List<COrderWarp> list = new List<COrderWarp>();
                DateTime min = DateTime.Parse(form["dateMin"]);
                DateTime max = DateTime.Parse(form["dateMax"]);
                List<int> oids = new List<int>();
                //string? json = HttpContext.Session.GetString(CDictionary.SK_MEMBER_LOGIN);
                //MemberWarp? m = JsonSerializer.Deserialize<MemberWarp>(json);
                oids = _context.Orders.Where(n => n.MemberId == id).Select(n => n.OrderId).ToList();

                //分頁
                int pageSize = 5;
                int total = _context.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).Skip(((int)page - 1) * pageSize).Take(pageSize).Count();
                int totalPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                if (page > totalPage)
                    page = totalPage;
                if (page < 1)
                    page = 1;

                var q = _context.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).Skip(((int)page - 1) * pageSize).Take(pageSize).ToList();
                //var q = _db.Orders.Include(n => n.Pay).Include(n => n.Ship).Include(n => n.State).Include(n => n.OrderDetails).ThenInclude(n => n.Product).Where(n => n.OrderDate >= min && n.OrderDate <= max && oids.Contains(n.OrderId)).OrderByDescending(n => n.OrderDate).ToList();
                foreach (var o in q)
                {
                    COrderWarp od = new COrderWarp();
                    od.order = o;
                    list.Add(od);
                }
                return Ok(list);
            }

            return Ok();
        }



        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
