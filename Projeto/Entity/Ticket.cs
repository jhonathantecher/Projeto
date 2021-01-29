using System;

namespace Projeto.Entity
{
    public class Ticket
    {  
        public int TicketId { get; set; }
        public Veiculo Veiculo { get; set; }
        public string VeiculoId { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public double Valor { get; set; }
        public bool Excluido { get; set; }
    }
}
