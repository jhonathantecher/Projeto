using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
using Projeto.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        TicketService servico = new TicketService();

        [HttpGet("Tickets")]
        public async Task<List<Ticket>> Get()
        {
            return await servico.ListagemTickets();
        }

        [HttpGet("Ativos")]
        public async Task<List<Ticket>> GetAtivos()
        {
            return await servico.ListagemTicketsAtivos();
        }

        [HttpGet("Finalizados")]
        public async Task<List<Ticket>> GetFinalizados()
        {
            return await servico.ListagemTicketsFinalizados();
        }

        [HttpGet("{id}")]
        public async Task<Ticket> Get(int id)
        {
            return await servico.BuscarTicket(id);
        }

        [HttpPost("{placa}")]
        public async Task<bool> Post(string placa)
        {         
            return await this.servico.CadastrarTicket(placa);
        }

        [HttpPut("{id}")]
        public async Task<bool> Put(int id)
        {
            return await this.servico.FinalizarTicket(id);
        }
    }
}
