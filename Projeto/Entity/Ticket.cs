using System;
using System.Collections.Generic;

namespace Projeto.Entity
{
    public class Ticket
    {
        public string Id { get; private set; }
        public string Id_Veiculo { get; private set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public double Valor { get; set; }


        public List<Ticket> ListaTickets = new List<Ticket>();

        public Ticket(){}

        public Ticket(string id_Ticket, string id_Veiculo, DateTime dataEntrada)
        {
            Id = id_Ticket;
            Id_Veiculo = id_Veiculo;
            DataEntrada = dataEntrada;
            DataSaida = null;
            Valor = 0.00;
        }
    }
}
