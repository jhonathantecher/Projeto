using Microsoft.EntityFrameworkCore;
using Projeto.Data;
using Projeto.Entity;
using Projeto.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto.Service
{
    public class ClienteService
    {
        Context context = new Context();

        public void CadastrarCliente(Cliente cliente)
        {
            if (!ValidacaoCPF(cliente.Cpf) && cliente.Cpf != null)
                throw new Exception("CPF Invalido!");

            //Busca para verificar se o Cliente já existe.
            var clienteExiste = BuscarClienteCPF(cliente.Cpf);

            if (clienteExiste == null)
            {
                var cli = new Cliente(cliente.Cpf, cliente.Nome, (cliente.Cpf == null ? TipoCliente.Passante : TipoCliente.Fixo));

                this.context.Clientes.Add(cliente);
                this.context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cliente já existente!");
            }
        }

        public void AtuaizarCliente(Cliente cliente)
        {
            //Busca para verificar se o Cliente já existe.
            var cli = BuscarClienteId(cliente.Id);

            if (cli != null)
            {
                if (CompararCPF(cliente))
                {
                    this.context.Clientes.Update(cliente);
                    this.context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("CPF ja Utilizado!");
                }
            }
            else
            {
                throw new Exception("Cliente não existe!");
            }
        }

        private Cliente BuscarClienteId(int id)
        {
            return this.context.Clientes.AsNoTracking().Where(cliente => cliente.Id == id).FirstOrDefault();
        }

        private Cliente BuscarClienteCPF(string cpf)
        {
            return this.context.Clientes.AsNoTracking().Where(cliente => cliente.Cpf == cpf).FirstOrDefault();
        }

        //Verifica se o CPF do cliente não é igual ao de outro Cliente.
        private bool CompararCPF(Cliente cliente)
        {
            var cli = this.context.Clientes.AsNoTracking().Where(cli => cli.Cpf == cliente.Cpf && cli.Id != cliente.Id).FirstOrDefault();

            if (cli == null)
                return true;
            return false;
        }

        public List<Cliente> ListagemClientes()
        {
            return this.context.Clientes.AsNoTracking().ToList();
        }

        public bool ValidacaoCPF(string cpf)
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
