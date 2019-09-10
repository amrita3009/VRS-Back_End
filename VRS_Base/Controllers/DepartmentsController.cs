using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VRS_Base.Models;
using Newtonsoft.Json;
using Confluent.Kafka;
using System.Text;

namespace VRS_Base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {

        private readonly ProducerConfig _config;
        public DepartmentsController(ProducerConfig config)
        {
            this._config = config;

        }

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
        public ActionResult<string> Get([FromQuery]int id)
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

        //[HttpPost]
        //public IActionResult Post([FromBody] string value)
        //{
        //    using (var db = new IshaMasterContext())
        //    {
        //        var newDept = new Dept();
        //        newDept.DeptName = value;
        //        db.Dept.Add(newDept);
        //        var count = db.SaveChanges();
        //        return Ok();
        //    }
        //}
        // POST api/values
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] string value)
        {
            string serializedOrder = JsonConvert.SerializeObject(value);
            Console.WriteLine("========");
            Console.WriteLine("Info: DepartmentsController => Post => Recieved a new deparment details:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");
            Dept newDept;
            using (var db = new IshaMasterContext())
            {
               newDept = new Dept();
                newDept.DeptName = value;
                db.Dept.Add(newDept);
                var count = db.SaveChanges();
            }

            var message = new StringBuilder();
            message.Append("{ \n event: \n DepartmentCreated \n entity: Department \n value:{ \n");
            message.Append(" ID: ");
            message.Append(newDept.DeptId);
            message.Append(",\n Name: ");
            message.Append(serializedOrder);
            message.Append("\n} \n }");

            var producer = new ProducerWrapper(this._config, "DepartmentCreated");
            await producer.writeMessage(message.ToString());

            return Created(Url.RouteUrl(newDept.DeptId), "Your deparment " + newDept.DeptId+" is created.");
        }

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] string value)
        //{
        //    using (var db = new IshaMasterContext())
        //    {
        //        Dept dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
        //        if(dept == null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            dept.DeptName = value;
        //            db.Dept.Update(dept);
        //            var count = db.SaveChanges();
        //            return Ok();
        //        }
        //    }
        //}

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromQuery]int id, [FromBody] string value)
        {
            string serializedOrder = JsonConvert.SerializeObject(value);
            Console.WriteLine("========");
            Console.WriteLine("Info: DepartmentsController => Update => Recieved a updated deparment details:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");


            using (var db = new IshaMasterContext())
            {
                Dept dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
                if (dept == null)
                {
                    return NotFound();
                }
                else
                {
                    dept.DeptName = value;
                    db.Dept.Update(dept);
                    var count = db.SaveChanges();
                   // return Ok();
                }
            }

            var message = new StringBuilder();
            message.Append("{ \n event: \n DepartmentUpdated \n entity: Department \n value: { \n");
            message.Append(" ID: ");
            message.Append(id);
            message.Append(",\n Name: ");
            message.Append(serializedOrder);
            message.Append("\n} }");

            var producer = new ProducerWrapper(this._config, "DepartmentUpdated");
            await producer.writeMessage(message.ToString());

            return Created("departmentId", "Your deparment " + id + " is updated.");

        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery]int id)
        {
            string serializedOrder = JsonConvert.SerializeObject(id);
            Console.WriteLine("========");
            Console.WriteLine("Info: DepartmentsController => Deleted => Recieved a deleted deparment details:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");
            Dept dept;
            StringBuilder message = new StringBuilder();
            using (var db = new IshaMasterContext())
            {
                dept = db.Dept.FirstOrDefault(d => d.DeptId == id);
                if (dept == null)
                {
                    message.Append("{ \n event: \n DepartmentDeleted \n entity: Department \n value: \n");
                    message.Append("Department record not found");
                    message.Append("\n }");

                }
                else
                {
                    db.Dept.Remove(dept);
                    var count = db.SaveChanges();
                    message = new StringBuilder();
                    message.Append("{ \n event: \n DepartmentDeleted \n entity: Department \n value: { \n");
                    message.Append(" ID: ");
                    message.Append(id);
                    message.Append(",\n Name: ");
                    message.Append(dept.DeptName);
                    message.Append("\n} \n }");
                }
            }

            var producer = new ProducerWrapper(this._config, "DepartmentDeleted");
            await producer.writeMessage(message.ToString());

            return Created("departmentId", "Your deparment " + id + " is deleted.");

        }
         
    }
}
