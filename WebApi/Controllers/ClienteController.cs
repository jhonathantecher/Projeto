using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
using Projeto.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        ClienteService servico = new ClienteService();

        [HttpGet]
        public async Task<List<Cliente>> Get()
        {
            return await this.servico.ListagemClientes();
        }

        [HttpGet("{cpf}")]
        public async Task<Cliente> Get(string cpf)
        {
            return await this.servico.BuscarClienteCPF(cpf);
        }

        [HttpPost]
        public async Task<bool> Post(Cliente cliente)
        {
            return await this.servico.CadastrarCliente(cliente);
        }

        [HttpPut]
        public async Task<bool> Put(Cliente cliente)
        {          
            return await this.servico.AtuaizarCliente(cliente);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await this.servico.ExcluirCliente(id);
        }
    }
}
