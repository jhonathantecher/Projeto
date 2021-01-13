using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Entity
{
    public class Veiculo
    {
        public string Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public List<Ticket> Ticktes { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public bool Excluido { get; set; }
    }
}
