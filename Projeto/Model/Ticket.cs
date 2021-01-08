using System;
using System.Collections.Generic;

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

        public Ticket(){}

        public Ticket(string placa, Veiculo veiculo, DateTime dataEntrada)
        {
            VeiculoId = placa;
            Veiculo = veiculo;
            DataEntrada = dataEntrada;
            DataSaida = null;
            Valor = 0.00;
        }

        public Ticket(int id, Veiculo veiculo, string placa, DateTime dataEntrada, DateTime? dataSaida, double valor)
        {
            Id = id;
            Veiculo = veiculo;
            VeiculoId = placa;
            DataEntrada = dataEntrada;
            DataSaida = dataSaida;
            Valor = valor;
        }
    }
}
