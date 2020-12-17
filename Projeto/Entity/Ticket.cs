using System;
using System.Collections.Generic;

namespace Projeto.Entity
{
    public class Ticket
    {
        public string Id { get; set; }
        public string Id_Veiculo { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public double Valor { get; set; }


        public Ticket(){}

        public Ticket(string id, string id_Veiculo, DateTime dataEntrada)
        {
            Id = id;
            Id_Veiculo = id_Veiculo;
            DataEntrada = dataEntrada;
            DataSaida = null;
            Valor = 0.00;
        }

        public Ticket(string id, string id_Veiculo, DateTime dataEntrada, DateTime? dataSaida, double valor)
        {
            Id = id;
            Id_Veiculo = id_Veiculo;
            DataEntrada = dataEntrada;
            DataSaida = dataSaida;
            Valor = valor;
        }
    }
}
