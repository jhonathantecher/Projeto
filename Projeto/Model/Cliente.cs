using Projeto.Entity.Enum;
using System.Collections.Generic;

namespace Projeto.Entity
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public TipoCliente TipoCliente { get; set; }

        public Cliente() {}
        public Cliente(string cpf, string nome, TipoCliente tipoCliente)
        {
            Cpf = cpf;
            Nome = nome;
            TipoCliente = tipoCliente;
        }
    }
}
