using Microsoft.AspNetCore.Mvc;
using Projeto.Entity;
using Projeto.Service;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        TicketService servico = new TicketService();

        // GET: api/<TicketController>
        [HttpGet("Finalizados")]
        public List<Ticket> Get()
        {
            return servico.ListagemTickets();
        }

        // GET: api/<TicketController>
        [HttpGet("Ativos")]
        public List<Ticket> GetAtivos()
        {
            return servico.ListagemTicketsAtivos();
        }

        // GET: api/<TicketController>
        [HttpGet("Finalizados")]
        public List<Ticket> GetFinalizados()
        {
            return servico.ListagemTicketsAtivos();
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TicketController>
        [HttpPost]
        public ActionResult Post(Ticket ticket)
        {
            try
            {               
                this.servico.CadastrarTicket(ticket);

                return Ok("Cadastrado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] Ticket ticket)
        {
            try
            {
                this.servico.FinalizarTicket(ticket);

                return Ok("Atualizado com Sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
