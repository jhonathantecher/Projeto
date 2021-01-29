using System;

namespace Projeto.Model
{
    public class TicketBuscaModel
    {
        public int Numero { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public double Valor { get; set; }
    }
}
