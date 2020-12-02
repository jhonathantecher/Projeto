﻿namespace Projeto.Entity
{
    public class Patio
    {
        public int Id_Patio { get; set; }
        public int Capacidade_Total { get; set; }
        public int Vagas_Ocupadas { get; set; }
        public int Vagas_Disponiveis { get; set; }

        public Patio()
        {
            Id_Patio = 1;
            Capacidade_Total = 3;
            Vagas_Ocupadas = 0;
            Vagas_Disponiveis = Capacidade_Total;
        }
    }
}