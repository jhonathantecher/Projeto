using Projeto.Entity.Enum;
using System.Collections.Generic;

namespace Projeto.Entity
{
    public class Cliente
    {
        public string Id { get; private set; }
        public string Nome { get; set; }
        public TipoCliente TipoCliente { get; set; }


        public List<Cliente> ListaCliente = new List<Cliente>();

        public Cliente(){}

        public Cliente(string cpf, string nome, TipoCliente tipoCliente)
        {
            Id = cpf;
            Nome = nome;          
            TipoCliente = tipoCliente;
        }
    }
}
