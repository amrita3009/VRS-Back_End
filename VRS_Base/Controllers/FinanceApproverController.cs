using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VRS_Base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceApproverController : ControllerBase
    {

        private readonly ConsumerConfig _config;
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

    }
}