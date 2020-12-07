using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Entity
{
    public class Veiculo
    {
        public string Id { get; private set; }
        public string Id_Dono { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }


        public List<Veiculo> ListaVeiculos = new List<Veiculo>();

        public Veiculo(){}

        public Veiculo(string placa, string id_Cliente, string marca, string modelo)
        {
            Id = placa;
            Id_Dono = id_Cliente;
            Marca = marca;
            Modelo = modelo;
        }      
    }
}
