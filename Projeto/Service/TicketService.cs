using Projeto.Data;
using Projeto.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Projeto.Model;
using System.Text.RegularExpressions;

namespace Projeto.Service
{
    public class TicketService
    {
        Context context = new Context();

        #region "Operações"
        public async Task<bool> CadastrarTicket(TicketCadastroModel model)
        {
            var patio = await this.context.Tickets
                .Where(t => t.DataSaida == null && !t.Excluido)
                .Select(t => t.VeiculoId)
                .ToListAsync();

            if (patio.Count >= 50)
                throw new Exception("Pátio Lotado.");

            if (patio.Any(v => v == model.Placa))
                throw new Exception("O Veículo já está no Estacionamento.");

            if (!ValidacaoPlaca(model.Placa))
                throw new Exception("Placa Inválida.");

            var veiculo = await this.context.Veiculos
                .Where(v => v.VeiculoId == model.Placa)
                .FirstOrDefaultAsync();

            var ticket = new Ticket();
            ticket.DataEntrada = DateTime.Now;

            if (veiculo == null)
            {
                ticket.Veiculo = new Veiculo
                {
                    VeiculoId = model.Placa,
                    Marca = model.Marca,
                    Modelo = model.Modelo,
                    Cliente = new Cliente
                    {
                        Nome = model.Nome
                    }
                };
            }
            else
            {
                //Caso o Veículo exista mas esteja exluído, sobreescreve as informações e reativa.
                if (veiculo.Excluido)
                {
                    veiculo.Marca = model.Marca;
                    veiculo.Modelo = model.Modelo;
                    veiculo.Excluido = false;

                    var cliente = await this.context.Clientes
                        .Where(c => c.ClienteId == veiculo.ClienteId)
                        .FirstOrDefaultAsync();

                    cliente.Nome = model.Nome;

                    this.context.Clientes.Update(cliente);
                    this.context.Veiculos.Update(veiculo);
                }      
                ticket.Veiculo = veiculo;
            }
            this.context.Tickets.Add(ticket);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FinalizarTicket(int numero)
        {
            var ticket = await this.context.Tickets
                .Where(t => t.TicketId == numero)
                .FirstOrDefaultAsync();

            if (ticket == null)
                throw new Exception("Ticket não Encontrado.");

            if (ticket.DataSaida != null)
                throw new Exception("Ticket já Finalizado.");

            ticket.DataSaida = DateTime.Now;
            ticket.Valor = 1.50 * (Math.Ceiling(DateTime.Now.Subtract(ticket.DataEntrada).TotalMinutes / 15));

            this.context.Tickets.Update(ticket);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirTicket(int numero)
        {
            var ticket = await this.context.Tickets
               .Where(t => t.TicketId == numero)
               .FirstOrDefaultAsync();

            if (ticket == null)
                throw new Exception("Ticket não Encontrado.");

            ticket.Excluido = true;

            this.context.Tickets.Update(ticket);
            await this.context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region "Estacionamento"
        public async Task<List<EstacionamentoBuscaModel>> ListagemEstacionamento()
        {
            return await this.context.Tickets
                .Where(t => t.DataSaida == null && !t.Excluido)
                .Select(t => new EstacionamentoBuscaModel
                {
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    Nome = t.Veiculo.Cliente.Nome,
                    DataEntrada = t.DataEntrada
                })
                .ToListAsync();
        }
        #endregion

        public async Task<VeiculoModel> BuscarVeiculo(string placa)
        {
            return await this.context.Veiculos
                .Where(v => v.VeiculoId == placa && !v.Excluido)
                .Select(v => new VeiculoModel
                {
                    Placa = v.VeiculoId,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Nome = v.Cliente.Nome
                })
                .FirstOrDefaultAsync();
        }

        #region "Listagem Ticket's"
        public async Task<List<TicketBuscaModel>> ListagemTickets()
        {
            return await this.context.Tickets
                .Where(t => !t.Excluido)
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .OrderBy(t => t.Numero)
                .OrderBy(t => t.DataSaida)         
                .ToListAsync();
        }

        public async Task<List<TicketBuscaModel>> ListagemTicketsFinalizados()
        {
            return await this.context.Tickets
                .Where(t => t.DataSaida != null && !t.Excluido)
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .ToListAsync();
        }
        #endregion

        #region "Pesquisa Ticket's"
        public async Task<List<TicketBuscaModel>> PesquisarTickets(string pesquisa)
        {
            return await this.context.Tickets
                .Where(t => !t.Excluido)
                .Where(t => t.TicketId.ToString().Contains(pesquisa) ||
                            t.Veiculo.VeiculoId.Contains(pesquisa) ||
                            t.Veiculo.Marca.Contains(pesquisa) ||
                            t.Veiculo.Modelo.Contains(pesquisa))
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .OrderBy(ticket => ticket.DataSaida)
                .ToListAsync();
        }

        public async Task<List<TicketBuscaModel>> PesquisarTicketsFinalizados(string pesquisa)
        {
            return await this.context.Tickets
                .Where(t => t.DataSaida != null && !t.Excluido)
                .Where(t => t.TicketId.ToString().Contains(pesquisa) ||
                            t.Veiculo.VeiculoId.Contains(pesquisa) ||
                            t.Veiculo.Marca.Contains(pesquisa) ||
                            t.Veiculo.Modelo.Contains(pesquisa))
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .OrderBy(ticket => ticket.DataSaida)
                .ToListAsync();
        }

        public async Task<List<TicketBuscaModel>> ListagemTicketsFinalizadosPeriodo(string inicio, string fim)
        {
            if (!ValidacaoDateTime(inicio, fim))
                throw new Exception("Data Invalida.");

            if (!ValidacaoDateTimeFinal(inicio, fim))
                throw new Exception("A Data Final deve ser maior que a Data Inicial.");

            return await this.context.Tickets
                .Where(t => t.DataSaida != null && !t.Excluido &&
                            t.DataSaida >= DateTime.Parse(inicio) && t.DataSaida <= DateTime.Parse(fim))
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .ToListAsync();
        }

        public async Task<List<TicketBuscaModel>> PesquisarTicketsFinalizadosPeriodo(string inicio, string fim, string pesquisa)
        {
            if (!ValidacaoDateTime(inicio, fim))
                throw new Exception("Data Invalida.");

            if (!ValidacaoDateTimeFinal(inicio, fim))
                throw new Exception("A Data Final deve ser maior que a Data Inicial.");

            return await this.context.Tickets
                .Where(t => t.DataSaida != null && !t.Excluido &&
                            t.DataSaida >= DateTime.Parse(inicio) && t.DataSaida <= DateTime.Parse(fim))
                .Where(t => t.TicketId.ToString().Contains(pesquisa) ||
                            t.Veiculo.VeiculoId.Contains(pesquisa) ||
                            t.Veiculo.Marca.Contains(pesquisa) ||
                            t.Veiculo.Modelo.Contains(pesquisa))
                .Select(t => new TicketBuscaModel
                {
                    Numero = t.TicketId,
                    Placa = t.Veiculo.VeiculoId,
                    Marca = t.Veiculo.Marca,
                    Modelo = t.Veiculo.Modelo,
                    DataEntrada = t.DataEntrada,
                    DataSaida = t.DataSaida,
                    Valor = t.Valor
                })
                .ToListAsync();
        }
        #endregion

        #region "Validações"
        public bool ValidacaoPlaca(string placa)
        {
            Regex regex = new Regex(@"^[A-Z]{3}\d{4}$");

            if (regex.IsMatch(placa))
                return true;
            return false;
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
        #endregion
    }
}
