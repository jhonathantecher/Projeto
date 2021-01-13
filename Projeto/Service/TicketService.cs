using Projeto.Data;
using Projeto.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Projeto.Service
{
    public class TicketService
    {
        Context context = new Context();
        VeiculoService veiculoService = new VeiculoService();

        public async Task<bool> CadastrarTicket(string placa)
        {
            var veiculo = await veiculoService.BuscarVeiculo(placa);

            if (veiculo == null)
                throw new Exception("Veiculo não Encontrado!");

            if (await VerificarVeiculoEstacionado(placa))
                throw new Exception("O Veiculo já se encontra no Estacionamento!");

            if ((await ListagemEstacionamento()).Count >= 50)
                throw new Exception("Patio Lotado!");

            var ticket = new Ticket
            {
                VeiculoId = placa,
                DataEntrada = DateTime.Now
            };

            this.context.Tickets.Add(ticket);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FinalizarTicket(int ticketId)
        {
            var ticket = await BuscarTicket(ticketId);

            if (ticket == null)
                throw new Exception("Ticket não Encontrado!");

            if (ticket.DataSaida != null)
                throw new Exception("Ticket Inativo!");

            ticket.DataSaida = DateTime.Now;
            ticket.Valor = 1.50 * (Math.Ceiling(DateTime.Now.Subtract(ticket.DataEntrada).TotalMinutes / 15));

            this.context.Tickets.Update(ticket);
            await this.context.SaveChangesAsync();

            return true;
        }

        //Calcula o valor total arrecadado nos Ticket's em determinado Período.
        public async Task<double> ValorPorPeriodo(string dataInicial, string dataFinal)
        {
            if (!ValidacaoDateTime(dataInicial, dataFinal))
                throw new Exception("Data Invalida!");

            if (!ValidacaoDateTimeFinal(dataInicial, dataFinal))
                throw new Exception("A Data Final deve ser maior que a Data Inicial!");

            return await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.DataSaida != null &&
                    ticket.DataEntrada >= DateTime.Parse(dataInicial) &&
                    ticket.DataSaida <= DateTime.Parse(dataFinal))
                .SumAsync(ticket => ticket.Valor);
        }

        private bool ValidacaoDateTime(string dataInicial, string dataFinal)
        {
            if (DateTime.TryParse(dataInicial, out DateTime r) && DateTime.TryParse(dataFinal, out DateTime t))
                return true;
            return false;
        }

        private bool ValidacaoDateTimeFinal(string dataInicial, string dataFinal)
        {
            if (DateTime.Parse(dataFinal) >= DateTime.Parse(dataInicial))
                return true;
            else
                return false;
        }

        public async Task<Ticket> BuscarTicket(int ticketId)
        {
            return await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.Id == ticketId)
                .FirstOrDefaultAsync();
        }

        private async Task<bool> VerificarVeiculoEstacionado(string placa)
        {
            var veiculo = await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.DataSaida == null && ticket.VeiculoId == placa)
                .FirstOrDefaultAsync();

            if (veiculo != null)
                return true;
            return false;
        }

        public async Task<List<Ticket>> ListagemTickets()
        {
            return await this.context.Tickets
                .AsNoTracking()
                .OrderBy(ticket => ticket.DataSaida)
                .ToListAsync();

        }
        public async Task<List<Ticket>> ListagemTicketsAtivos()
        {
            return await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.DataSaida == null)
                .ToListAsync();
        }

        public async Task<List<Veiculo>> ListagemEstacionamento()
        {
            return await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.DataSaida == null)
                .Select(ticket => ticket.Veiculo)
                .ToListAsync();
        }

        public async Task<List<Ticket>> ListagemTicketsFinalizados()
        {
            return await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.DataSaida != null)
                .ToListAsync();
        }
    }
}
