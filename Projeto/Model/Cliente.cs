using Projeto.Entity.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Entity
{
    public class Cliente
    {
        public int Id { get; set; }
        public List<Veiculo> Veiculos { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        [Column("TipoCliente")]
        public TipoClienteEnum TipoClienteEnum { get; set; }
        public bool Excluido { get; set; }
    }
}
