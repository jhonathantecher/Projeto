using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Entity
{
    public class Veiculo
    {
        public string Id_Placa { get; private set; }
        public string Id_Dono { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }


        public List<Veiculo> ListaVeiculos = new List<Veiculo>();

        public Veiculo(){}

        public Veiculo(string id_Placa, string id_Dono, string marca, string modelo)
        {
            Id_Placa = id_Placa;
            Id_Dono = id_Dono;
            Marca = marca;
            Modelo = modelo;
        }      
    }
}
