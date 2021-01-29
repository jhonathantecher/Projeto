using Microsoft.AspNetCore.Mvc;
using Projeto.Model;
using Projeto.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        TicketService servico = new TicketService();

        [HttpGet("estacionamento")]
        public async Task<List<EstacionamentoBuscaModel>> ObterListagemEstacionamento()
        {
            return await servico.ListagemEstacionamento();
        }

        [HttpGet("tickets")]
        public async Task<List<TicketBuscaModel>> ObterListagemTickets()
        {
            return await servico.ListagemTickets();
        }

        [HttpGet("tickets/{pesquisa}")]
        public async Task<List<TicketBuscaModel>> PesquisarListagemTickets(string pesquisa)
        {
            return await servico.PesquisarTickets(pesquisa);
        }

        [HttpGet("tickets/finalizados")]
        public async Task<List<TicketBuscaModel>> ObterListagemTicketsFinalizados()
        {
            return await servico.ListagemTicketsFinalizados();
        }

        [HttpGet("tickets/finalizados/{pesquisa}")]
        public async Task<List<TicketBuscaModel>> PesquisarListagemTicketsFinalizados(string pesquisa)
        {
            return await servico.PesquisarTicketsFinalizados(pesquisa);
        } 

        [HttpGet("{placa}")]
        public async Task<VeiculoModel> BuscarVeiculo(string placa)
        {
            return await servico.BuscarVeiculo(placa);
        }

        [HttpPost]
        public async Task<bool> CadastrarTicket(TicketCadastroModel model)
        {         
            return await this.servico.CadastrarTicket(model);
        }

        [HttpPut("{numero}")]
        public async Task<bool> FinalizarTicket(int numero)
        {
            return await this.servico.FinalizarTicket(numero);
        }

        [HttpDelete("{numero}")]
        public async Task<bool> ExcluirTicket(int numero)
        {
            return await this.servico.ExcluirTicket(numero);
        }
    }
}
