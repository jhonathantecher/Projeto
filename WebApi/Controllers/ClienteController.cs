using Microsoft.AspNetCore.Mvc;
using Projeto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public void Get(int id)
        {
            ClienteService servico = new ClienteService();

            servico.CadastrarCliente("Julia da Silva", "1234567");
        }

        // POST api/values
        [HttpPost]
        public void Post()
        {
            ClienteService servico = new ClienteService();

            servico.CadastrarCliente("Julia da Silva", "1234567");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
