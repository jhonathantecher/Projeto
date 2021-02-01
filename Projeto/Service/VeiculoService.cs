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
               .Where(v => v.VeiculoId == model.Placa)
               .FirstOrDefaultAsync();

            if (veiculo == null)
                throw new Exception("O Veículo não Existe!");

            veiculo.Marca = model.Marca;
            veiculo.Modelo = model.Modelo;

            var cliente = await this.context.Clientes
               .Where(c => c.ClienteId == veiculo.ClienteId)
               .FirstOrDefaultAsync();

            cliente.Nome = model.Nome;

            this.context.Clientes.Update(cliente);
            this.context.Veiculos.Update(veiculo);

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirVeiculo(string placa)
        {
            var veiculo = await this.context.Veiculos
                .Where(v => v.VeiculoId == placa)
                .FirstOrDefaultAsync();

            if (veiculo == null)
                throw new Exception("O Veículo não Existe.");

            if (this.context.Tickets.Any(t => t.VeiculoId == placa && t.DataSaida == null && !t.Excluido))
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
                            .Where(v => !v.Excluido)
                            .Select(v => new VeiculoModel
                            {
                                Placa = v.VeiculoId,
                                Marca = v.Marca,
                                Modelo = v.Modelo,
                                Nome = v.Cliente.Nome
                            })
                            .ToListAsync();
        }

        public async Task<List<VeiculoModel>> PesquisarVeiculos(string pesquisa)
        {
            return await this.context.Veiculos
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
