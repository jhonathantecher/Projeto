using Projeto.Entity.Enum;
using System.Collections.Generic;

namespace Projeto.Entity
{
    public class Cliente
    {
        public string Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public TipoCliente TipoCliente { get; set; }
    }
}
