using MedSysApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedSysApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CPlanController : ControllerBase
    {
private readonly MedSysContext _context;
    public CPlanController(MedSysContext context)
    {
        _context = context;
    }

        // GET: api/<CPlanController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CPlanController>/5
        [HttpGet("{id}")]
        public string Get(int? planid)
        {
            //加mermber id

            var pl = _context.Plans.Where(p => p.PlanId == planid)
               .SelectMany(p => p.PlanRefs, (plan, project) => new { plan, project }).Where(p => p.project.PlanId == planid)
               .SelectMany(p => p.project.Project.Items, (prbg, it) => new { prbg.project.Project, it }).Where(p => p.Project.ProjectId == p.it.ProjectId)

               //.SelectMany(p => p.project.Project.Items, (projectid, item) => new { projectid, item }).Where(p => p.item.ProjectId == p.projectid.project.ProjectId)
               .Select(t => new
               {
                   planId = t.Project.PlanRefs.First().PlanId,
                   planName = t.Project.PlanRefs.First().Plan.PlanName,
                   projectid = t.Project.ProjectId,
                   ProjectName = (string)t.Project.ProjectName,
                   ProjectPrice = (double)t.Project.ProjectPrice,
                   itemId = t.it.ItemId,
                   ItemName = (string)t.it.ItemName,
                   ItemPrice=(int) t.it.ItemPrice,
               });
            //------datatable 轉json區--------
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("planId"));
            dt.Columns.Add(new DataColumn("planName"));

            dt.Columns.Add(new DataColumn("projectid"));
            dt.Columns.Add(new DataColumn("ProjectName"));
            dt.Columns.Add(new DataColumn("ProjectPrice"));
            dt.Columns.Add(new DataColumn("itemId"));
            dt.Columns.Add(new DataColumn("ItemName"));
            dt.Columns.Add(new DataColumn("ItemPrice"));
            foreach (var t in pl)
            {
                DataRow dr = dt.NewRow();

                dr["planId"] = t.planId;
                dr["PlanName"] = t.planName;

                dr["projectid"] = t.projectid;
                dr["ProjectName"] = t.ProjectName;
                dr["ProjectPrice"] = t.ProjectPrice;
                dr["itemId"] = t.itemId;
                dr["ItemName"] = t.ItemName;
                dr["ItemPrice"] = t.ItemPrice;
                dt.Rows.Add(dr);
            }
            DataTableToJsonConverter converter = new DataTableToJsonConverter();
            string js = converter.ConverterDataTableToJson(dt);

            //------datatable 轉json區--------

          
           
                string json = System.Text.Json.JsonSerializer.Serialize(pl);
               

                return js;
            
        }
        public class DataTableToJsonConverter
        {
            public string ConverterDataTableToJson(DataTable dataTable)
            {
                string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                return json;
            }

        }

        // POST api/<CPlanController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CPlanController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CPlanController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
