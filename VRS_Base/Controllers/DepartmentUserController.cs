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
    public class DepartmentUserController : ControllerBase
    {

        private readonly ProducerConfig _config;
        public DepartmentUserController(ProducerConfig config)
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
                var result = db.Vendor;
                return new JsonResult(result.ToList());
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Vendor> Get([FromQuery]int id)
        {
            //return "value";
            using (var VendorDb = new IshaMasterContext())
            {
                Vendor vendorResult = VendorDb.Vendor.FirstOrDefault(vendor => vendor.ID == id);
                if (vendorResult == null)
                {
                    return NotFound();
                }
                else
                {
                    return vendorResult;
                }
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Vendor value)
        {
            string serializedOrder = JsonConvert.SerializeObject(value);
            Console.WriteLine("========");
            Console.WriteLine("Info: DepartmentUserController => Post => Recieved a new vendor details:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");
            Vendor newVendor;
            using (var db = new IshaMasterContext())
            {
                newVendor = new Vendor(value);
                db.Vendor.Add(newVendor);
                var count = db.SaveChanges();
            }

            var message = new StringBuilder();
            message.Append("{ \n event: \n VendorRequested \n entity: vendor \n value:{ \n");
            message.Append(" ID: ");
            message.Append(newVendor.ID);
            message.Append(",\n details: ");
            message.Append(serializedOrder);
            message.Append("\n} \n }");

            var producer = new ProducerWrapper(this._config, "VendorRequested");
            await producer.writeMessage(message.ToString());

            return Created(Url.RouteUrl(newVendor.ID), "Your Vendor " + newVendor.Name + " with mobile number "+newVendor.MobileNumber+" is created.");
        }

    }
}