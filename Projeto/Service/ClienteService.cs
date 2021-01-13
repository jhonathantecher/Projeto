using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Entity;
using Projeto.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Service
{
    public class ClienteService
    {
        Context context = new Context();

        public async Task<bool> CadastrarCliente(Cliente cliente)
        {
            if (!ValidacaoCPF(cliente.Cpf) && cliente.Cpf != null)
                throw new Exception("CPF Invalido!");

            var clienteExiste = await BuscarClienteCPF(cliente.Cpf);

            if (clienteExiste != null)
                throw new Exception("Cliente já existente!");

            cliente = new Cliente
            {
                Cpf = cliente.Cpf,
                Nome = cliente.Nome,
                TipoClienteEnum = cliente.Cpf == null ? TipoClienteEnum.Padrao : TipoClienteEnum.Mensalista,
                Excluido = false
            };

            this.context.Clientes.Add(cliente);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AtuaizarCliente(Cliente cliente)
        {
            var cli = await BuscarClienteId(cliente.Id);

            if (cli == null)
                throw new Exception("Cliente não existe!");

            if (!await CompararCPF(cliente))
                throw new Exception("CPF ja Utilizado!");

            this.context.Clientes.Update(cliente);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirCliente(int clienteId)
        {
            var cliente = await BuscarClienteId(clienteId);

            if (cliente == null)
                throw new Exception("Cliente não existe!");

            cliente.Excluido = true;

            this.context.Clientes.Update(cliente);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<Cliente> BuscarClienteId(int id)
        {
            return await this.context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Cliente> BuscarClienteCPF(string cpf)
        {
            return await this.context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.Cpf == cpf && cpf != null)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Cliente>> ListagemClientes()
        {
            return await this.context.Clientes
                .Where(cliente => !cliente.Excluido)
                .AsNoTracking()
                .ToListAsync();
        }

        //Verifica se o CPF do cliente não é igual ao de outro Cliente.
        private async Task<bool> CompararCPF(Cliente cliente)
        {
            var cli = await this.context.Clientes
                .AsNoTracking()
                .Where(cli => cli.Cpf == cliente.Cpf && cli.Id != cliente.Id)
                .FirstOrDefaultAsync();

            if (cli == null)
                return true;
            return false;
        }

        private bool ValidacaoCPF(string cpf)
        {
            if (cpf != null)
            {
                if (cpf.Length == 11 && long.TryParse(cpf, out long numero))
                    return true;
                return false;
            }
            return false;
        }
    }
}
