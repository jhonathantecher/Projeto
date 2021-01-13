using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Entity;
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
        ClienteService clienteService = new ClienteService();

        public async Task<bool> CadastrarVeiculo(Veiculo veiculo)
        {
            if (!ValidacaoPlaca(veiculo.Id))
                throw new Exception("Placa Invalida!");

            //Verifica se o Veiculo nao Existe.
            var vei = await BuscarVeiculo(veiculo.Id);
            if (vei != null)
                throw new Exception("Veiculo ja existente!");

            //Verifica se o Dono do Veiculo Existe.
            var cliente = await this.clienteService.BuscarClienteId(veiculo.ClienteId);
            if (cliente == null)
                throw new Exception("Cliente Inexistente!");

            this.context.Add(veiculo);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AtualizarVeiculo(Veiculo veiculo)
        {
            //Verifica se o Veiculo Existe.
            var vei = await BuscarVeiculo(veiculo.Id);

            if (vei == null)
                throw new Exception("Veiculo Inexistente!");

            this.context.Veiculos.Update(veiculo);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirVeiculo(string placa)
        {
            //Verifica se o Veiculo Existe.
            var veiculo = await BuscarVeiculo(placa);

            if (veiculo == null)
                throw new Exception("Veiculo Inexistente!");

            veiculo.Excluido = true;

            this.context.Veiculos.Update(veiculo);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<Veiculo> BuscarVeiculo(string veiculoId)
        {
            return await this.context.Veiculos
                .AsNoTracking()
                .Where(veiculo => veiculo.Id == veiculoId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Veiculo>> ListagemVeiculos()
        {
            return await this.context.Veiculos
                .AsNoTracking()
                .ToListAsync();
        }

        public bool ValidacaoPlaca(string placa)
        {
            Regex regex = new Regex(@"^[A-Z]{3}\d{4}$");

            if (regex.IsMatch(placa))
                return true;
            return false;
        }
    }
}
