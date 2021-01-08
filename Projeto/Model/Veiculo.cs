using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Entity
{
    public class Veiculo
    {
        public string Id { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }

        public Veiculo(){}

        public Veiculo(string placa, Cliente cliente, int clienteId, string marca, string modelo)
        {
            Id = placa;
            Cliente = cliente;
            ClienteId = clienteId;
            Marca = marca;
            Modelo = modelo;
        }      
    }
}
