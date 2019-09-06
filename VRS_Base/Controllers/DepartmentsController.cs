using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VRS_Base.Models;

namespace VRS_Base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            //var result = new string[] { "a", "b", "c", "d", "e"};
            //return new JsonResult(result.ToList());
            using (var db = new IshaMasterContext())
            {
                var result = db.Dept;
                return new JsonResult(result.ToList());
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            //return "value";
            using (var db = new IshaMasterContext())
            {
                Dept dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
                if (dept == null)
                {
                    return NotFound();
                }
                else
                {
                    return dept.DeptName;
                }
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            using (var db = new IshaMasterContext())
            {
                var newDept = new Dept();
                newDept.DeptName = value;
                db.Dept.Add(newDept);
                var count = db.SaveChanges();
                return Ok();
            }          
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            using (var db = new IshaMasterContext())
            {
                Dept dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
                if(dept == null)
                {
                    return NotFound();
                }
                else
                {
                    dept.DeptName = value;
                    db.Dept.Update(dept);
                    var count = db.SaveChanges();
                    return Ok();
                }
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var db = new IshaMasterContext())
            {
                Dept dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
                if (dept == null)
                {
                    return NotFound();
                }
                else
                {
                    db.Dept.Remove(dept);
                    var count = db.SaveChanges();
                    return Ok();
                }
            }
        }

    }
}
