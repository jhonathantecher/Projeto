using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Entity
{
    public class Ticket
    {  
        public int Id { get; set; }
        public Veiculo Veiculo { get; set; }
        public string VeiculoId { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public double Valor { get; set; }

    }
}
