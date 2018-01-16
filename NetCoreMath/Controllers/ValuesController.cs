using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCoreMath.MathJob;

namespace NetCoreMath.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        string hostName;
        public ValuesController(IOptions<AppConfig> setting)
        {
            this.hostName = setting.Value.HostName;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            for (int i = 0; i < id; i++)
            {
                MathCoreService.blockingCollection.Add(i);
            }
            Console.WriteLine($"{hostName}-{DateTime.Now}服务器 【收到】 {id} 个计算服务，等待任务队列执行...");
            return new { Serive = hostName, Message = $"服务器收到 {id} 个计算服务 " };
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
