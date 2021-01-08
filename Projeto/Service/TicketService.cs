using Projeto.Data;
using Projeto.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Projeto.Service
{
    public class TicketService
    {
        Context context = new Context();

        //Realiza a Operação de Inclusão de um Ticket.
        public void CadastrarTicket(Ticket ticket)
        {
            //Verifica se o Veiculo existe.
            var veiculo = BuscarVeiculo(ticket.VeiculoId);

            if (veiculo != null)
            {
                //Verifica se o Veiculo informado não esta no Patio.
                //Dessa forma um Ticket não pode ser gerado duas vezes para o mesmo Veiculo.
                if (!VerificarVeiculoEstacionado(ticket.VeiculoId))
                {
                    //Verifica se há Vagas disponiveis.
                    if (ListagemEstacionamento().Count < 50)
                    {
                        //Se o veiculo existir e houverem Vagas Disponiveis, o Ticket é gerado.
                        ticket = new Ticket(ticket.VeiculoId, veiculo, DateTime.Now);

                        this.context.Tickets.Add(ticket);
                        this.context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception("Patio Lotado!");
                    }
                }
                else
                {
                    throw new Exception("O Veiculo já se encontra no Estacionamento!");
                }
            }
            else
            {
                throw new Exception("Veiculo não Encontrado!");
            }
        }

        //Realiza a Operação de encerramento de um Ticket.
        public void FinalizarTicket(Ticket ticket)
        {
            //Busca o Ticket para Verificar se ele existe.
            var tic = BuscarTicket(ticket.Id);

            if (ticket != null)
            {
                //Verifica se o Ticket esta ativo para poder ser finalizado.
                if (ticket.DataSaida == null)
                {
                    ticket.DataSaida = DateTime.Now;
                    ticket.Valor = (1.50 * (Math.Ceiling(DateTime.Now.Subtract(ticket.DataEntrada).TotalMinutes / 15)));

                    this.context.Tickets.Update(ticket);
                    this.context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Ticket Inativo!");
                }
            }
            else
            {
                throw new Exception("Ticket não Encontrado!");
            }
        }

        //Calcula o valor total arrecadado nos Ticket's em determinado Período.
        public double ValorPorPeriodo(string dataInicial, string dataFinal)
        {
            if (!ValidacaoDateTime(dataInicial, dataFinal))
                throw new Exception("Data Invalida!");

            if (!ValidacaoDateTimeFinal(dataInicial, dataFinal))
                throw new Exception("A Data Final deve ser maior que a Data Inicial!");

            return this.context.Tickets.AsNoTracking().Where(ticket => ticket.DataSaida != null && 
                                                ticket.DataEntrada >= DateTime.Parse(dataInicial) && 
                                                ticket.DataSaida <= DateTime.Parse(dataFinal))
                                                .Sum(ticket => ticket.Valor);
        }

        public bool ValidacaoDateTime(string dataInicial, string dataFinal)
        {
            if (DateTime.TryParse(dataInicial, out DateTime r) && DateTime.TryParse(dataFinal, out DateTime t))
                return true;
            return false;
        }

        public bool ValidacaoDateTimeFinal(string dataInicial, string dataFinal)
        {
            if (DateTime.Parse(dataFinal) >= DateTime.Parse(dataInicial))
                return true;
            else
                return false;
        }

        private Veiculo BuscarVeiculo(string placa)
        {
            return this.context.Veiculos.AsNoTracking().Where(veiculo => veiculo.Id == placa).FirstOrDefault();
        }

        private Ticket BuscarTicket(int ticketId)
        {
            return this.context.Tickets.AsNoTracking().Where(ticket => ticket.Id == ticketId).FirstOrDefault();
        }

        //Verifica se um determinado Veiculo se encontra no estacionamento.
        private bool VerificarVeiculoEstacionado(string placa)
        {
            var veiculo = this.context.Tickets.AsNoTracking().Where(ticket => ticket.DataSaida == null && ticket.VeiculoId == placa).FirstOrDefault();

            if (veiculo != null)
                return true;
            return false;
        }

        public List<Ticket> ListagemTickets()
        {
            return this.context.Tickets.AsNoTracking().OrderBy(ticket => ticket.DataSaida).ToList();

        }
        public List<Ticket> ListagemTicketsAtivos()
        {
            return this.context.Tickets.AsNoTracking().Where(ticket => ticket.DataSaida == null).ToList();
        }

        public List<Veiculo> ListagemEstacionamento()
        {
            return this.context.Tickets.AsNoTracking().Where(ticket => ticket.DataSaida == null).Select(ticket => ticket.Veiculo).ToList();
        }

        public List<Ticket> ListagemTicketsFinalizados()
        {
            return this.context.Tickets.AsNoTracking().Where(ticket => ticket.DataSaida != null).ToList();
        }
    }
}
