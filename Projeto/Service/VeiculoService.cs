using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Projeto.Service
{
    public class VeiculoService
    {
        Context context = new Context();

        public void CadastrarVeiculo(Veiculo veiculo)
        {
            if (!ValidacaoPlaca(veiculo.Id))
                throw new Exception("Placa Invalida!");

            //Verifica se o Veiculo nao Existe.
            var vei = BuscarVeiculo(veiculo.Id);
            if (vei != null)
                throw new Exception("Veiculo ja Existe!");

            //Verifica se o Dono do Veiculo Existe.
            var cliente = BuscarCliente(veiculo.ClienteId);
            if (cliente == null)
                throw new Exception("Cliente Inexistente!");

            vei = new Veiculo(veiculo.Id, cliente, cliente.Id, veiculo.Marca, veiculo.Modelo);
            this.context.Add(veiculo);
            this.context.SaveChangesAsync();
        }

        public void AtualizarVeiculo(Veiculo veiculo)
        {
            //Verifica se o Veiculo Existe.
            var vei = BuscarVeiculo(veiculo.Id);

            if (vei == null)
                throw new Exception("Veiculo Inexistente!");

            this.context.Veiculos.Update(veiculo);
            this.context.SaveChangesAsync();
        }

        public Veiculo BuscarVeiculo(string veiculoId)
        {
            return this.context.Veiculos.AsNoTracking().Where(veiculo => veiculo.Id == veiculoId).FirstOrDefault();

        }
        private Cliente BuscarCliente(int clienteId)
        {
            return this.context.Clientes.AsNoTracking().Where(cliente => cliente.Id == clienteId).FirstOrDefault();
        }
        public List<Veiculo> ListagemVeiculos()
        {
            return this.context.Veiculos.AsNoTracking().ToList();
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
