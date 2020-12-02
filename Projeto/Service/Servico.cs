using Projeto.Entity;
using Projeto.Entity.Enum;
using System.Threading;
using System;
using System.Collections.Generic;

namespace Projeto.Service
{
    public class Servico
    {
        Cliente cliente = new Cliente();
        Veiculo veiculo = new Veiculo();
        Ticket ticket = new Ticket();
        Patio patio = new Patio();

        public void CadastrarCliente(string nome, string cpf)
        {
            Cliente cli = BuscarCliente(cpf);

            if (cli == null)
            {
                cli = new Cliente(cpf, nome, TipoCliente.Fixo);
                cliente.ListaCliente.Add(cli);

                System.Console.WriteLine("\nCadastrado com Sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                System.Console.WriteLine("\nCliente já existente!");
                Thread.Sleep(1000);
            }

        }

        public string ListagemClientes()
        {
            string listaCli = "";

            foreach (var cliente in cliente.ListaCliente)
            {
                listaCli += cliente.Id + " - " + cliente.Nome + " - " + cliente.TipoCliente + "\n";

            }
            return listaCli;
        }

        public void CadastrarVeiculo(string placa, string marca, string modelo, string tipoCliente, string dono_Id, string nome)
        {
            Cliente cli;

            if (dono_Id != null)
            {
                cli = BuscarCliente(dono_Id);

                if (cli == null)
                {
                    Console.WriteLine("\nCliente Inexistente!");
                    Thread.Sleep(1000);
                    return;
                }
            }
            else
            {
                cli = new Cliente(
                    (cliente.ListaCliente.Count + 1).ToString(),
                    nome,
                    TipoCliente.Passante);

                cliente.ListaCliente.Add(cli);
            }

            Veiculo vei = new Veiculo(placa, cli.Id, marca, modelo);
            veiculo.ListaVeiculos.Add(vei);

            Console.WriteLine("\nCadastrado com Sucesso!");
            Thread.Sleep(1000);
        }

        public string ListagemVeiculos()
        {
            string ListVei = "";

            foreach (var veiculo in veiculo.ListaVeiculos)
            {
                Cliente dono = BuscarCliente(veiculo.Id_Dono);

                ListVei += "Placa: " + veiculo.Id_Placa + "\n" +
                      "Marca: " + veiculo.Marca + "\n" +
                      "Modelo: " + veiculo.Modelo + "\n" +
                      "Dono: " + dono.Id + " - " + dono.Nome +
                      "\n\n";
            }

            return ListVei;
        }

        public void CadastrarTicket(string placa)
        {
            Veiculo vei = BuscarVeiculo(placa);

            if (vei != null)
            {
                if (patio.Vagas_Ocupadas < patio.Capacidade_Total)
                {
                    Ticket tic = new Ticket(
                        (ticket.ListaTickets.Count + 1).ToString(),
                        vei.Id_Placa,
                        DateTime.Now);

                    ticket.ListaTickets.Add(tic);
                    patio.Vagas_Ocupadas++;
                    patio.Vagas_Disponiveis--;

                    Console.WriteLine("\nTicket Gerado com Sucesso!");
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("\nPatio Lotado!");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                System.Console.WriteLine("\nVeiculo não Encontrado!");
                Thread.Sleep(1000);
            }
        }

        public void FecharTicket(string id_Ticket)
        {
            int index = BuscarIndexTicket(id_Ticket);

            if (index >= 0)
            {           
                TimeSpan duracao = DateTime.UtcNow.Subtract(ticket.ListaTickets[index].DataEntrada);
                ticket.ListaTickets[index].DataSaida = DateTime.UtcNow;

                ticket.ListaTickets[index].Valor = 1.50 * (Math.Ceiling(duracao.TotalMinutes / 15));

                patio.Vagas_Ocupadas--;
                patio.Vagas_Disponiveis++;

                System.Console.WriteLine("\nTicket Finalizado com Sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                System.Console.WriteLine("\nTicket não Encontrado!");
                Thread.Sleep(1000);
            }
        }

        public string ListagemTicketsAtivos()
        {
            string listaTic = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida == null)
                {
                    Veiculo vei = BuscarVeiculo(ticket.Id_Veiculo);
                    Cliente cli = BuscarCliente(vei.Id_Dono);

                    listaTic += "Ticket: " + ticket.Id_Ticket + "\n" +
                    "Veiculo: " + ticket.Id_Veiculo + " - " +
                    vei.Marca + " - " + vei.Modelo + "\n" +
                    "Dono: " + cli.Id + " - " + cli.Nome + "\n" +
                    "Entrada: " + ticket.DataEntrada + "\n" +
                    "Saida: " + ticket.DataSaida +
                    "\n\n";
                }
            }

            return listaTic;
        }

        public string ListagemTicketsFinalizados()
        {
            string listaTic = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida != null)
                {
                    Veiculo vei = BuscarVeiculo(ticket.Id_Veiculo);
                    Cliente cli = BuscarCliente(vei.Id_Dono);

                    listaTic += "Ticket: " + ticket.Id_Ticket + "\n" +
                    "Veiculo: " + ticket.Id_Veiculo + " - " +
                    vei.Marca + " - " + vei.Modelo + "\n" +
                    "Dono: " + cli.Id + " - " + cli.Nome + "\n" +
                    "Entrada: " + ticket.DataEntrada + "\n" +
                    "Saida: " + ticket.DataSaida + "\n" +
                    "Valor: " + ticket.Valor +
                    "\n\n";
                }
            }

            return listaTic;
        }

        private Veiculo BuscarVeiculo(string id_Veiculo)
        {
            for (int i = 0; i <= veiculo.ListaVeiculos.Count - 1; i++)
            {
                if (veiculo.ListaVeiculos[i].Id_Placa == id_Veiculo)
                {
                    return veiculo.ListaVeiculos[i];
                }
            }
            return null;
        }

        private Cliente BuscarCliente(string id_Cliente)
        {
            for (int i = 0; i <= cliente.ListaCliente.Count - 1; i++)
            {
                if (cliente.ListaCliente[i].Id == id_Cliente)
                {
                    return cliente.ListaCliente[i];
                }
            }
            return null;
        }

        private int BuscarIndexTicket(string id_Ticket)
        {
            for (int i = 0; i <= ticket.ListaTickets.Count - 1; i++)
            {
                if (ticket.ListaTickets[i].Id_Ticket == id_Ticket)
                {
                    return i;
                }
            }
            return -1;
        }

        public Patio PatioEstacionamento()
        {
            return patio;
        }

        public string ListaEstacionamento()
        {
            string listaEstacionamento = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida == null)
                {
                    Veiculo vei = BuscarVeiculo(ticket.Id_Veiculo);
                    Cliente cli = BuscarCliente(vei.Id_Dono);

                    listaEstacionamento += $"Veiculo: {ticket.Id_Veiculo} - {vei.Marca} - {vei.Modelo} \n Dono: {cli.Id} - {cli.Nome} \n\n";
                }
            }

            return listaEstacionamento;
        }

        public double ValorPorPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            double total = 0.00;
            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida >= dataInicial && ticket.DataSaida <= dataFinal)
                {
                    total += ticket.Valor;
                }
            }

            return total;
        }
    }
}
