using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Entity;
using Projeto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Projeto.Service
{
    public class VeiculoService
    {
        Context context = new Context();

        #region "Operações"
        public async Task<bool> AtualizarVeiculo(VeiculoModel model)
        {
            if (!ValidacaoPlaca(model.Placa))
                throw new Exception("Placa Inválida!");

            var veiculo = await this.context.Veiculos
               .AsNoTracking()
               .Where(v => v.VeiculoId == model.Placa)
               .FirstOrDefaultAsync();

            if (veiculo == null)
                throw new Exception("O Veículo não Existe!");

            veiculo = new Veiculo
            {
                VeiculoId = model.Placa,
                Marca = model.Marca,
                Modelo = model.Modelo,
                Cliente = new Cliente
                {
                    ClienteId = model.ClienteId,
                    Nome = model.Nome
                }
            };

            this.context.Veiculos.Update(veiculo);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirVeiculo(string placa)
        {
            var veiculo = await this.context.Veiculos
                .AsNoTracking()
                .Where(veiculo => veiculo.VeiculoId == placa)
                .FirstOrDefaultAsync();

            if (veiculo == null)
                throw new Exception("O Veículo não Existe.");

            var ticket = await this.context.Tickets
                .AsNoTracking()
                .Where(ticket => ticket.VeiculoId == placa && ticket.DataSaida == null && !ticket.Excluido)
                .FirstOrDefaultAsync();

            if (ticket != null)
                throw new Exception("O Veículo possui Ticket's em aberto.");

            veiculo.Excluido = true;

            this.context.Veiculos.Update(veiculo);
            await this.context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region "Buscas"
        public async Task<List<VeiculoModel>> ListagemVeiculos()
        {
            return await this.context.Veiculos
                            .AsNoTracking()
                            .Where(v => !v.Excluido)
                            .Select(v => new VeiculoModel
                            {
                                Placa = v.VeiculoId,
                                Marca = v.Marca,
                                Modelo = v.Modelo,
                                ClienteId = v.ClienteId,
                                Nome = v.Cliente.Nome
                            })
                            .ToListAsync();
        }

        public async Task<List<VeiculoModel>> PesquisarVeiculos(string pesquisa)
        {
            return await this.context.Veiculos
                .AsNoTracking()
                .Where(v => !v.Excluido)
                .Where(v => v.VeiculoId.Contains(pesquisa) ||
                            v.Marca.Contains(pesquisa) ||
                            v.Modelo.Contains(pesquisa) ||
                            v.Cliente.Nome.Contains(pesquisa))
                .Select(v => new VeiculoModel
                {
                    Placa = v.VeiculoId,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Nome = v.Cliente.Nome
                })
                .ToListAsync();
        }
        #endregion

        public bool ValidacaoPlaca(string placa)
        {
            Regex regex = new Regex(@"^[A-Z]{3}\d{4}$");

            if (regex.IsMatch(placa))
                return true;
            return false;
        }
    }
}
