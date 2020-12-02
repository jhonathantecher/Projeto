using Projeto.Entity;
using Projeto.Service;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Projeto
{
    class Program
    {
        static Servico servico = new Servico();

        static void Main(string[] args)
        {
            MenuPrincipal();
        }
        //
        static void MenuPrincipal()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("-----ESTACIONAMENTO TECHER-----");
                Console.WriteLine("1 - Controle de Clientes");
                Console.WriteLine("2 - Controle de Veiculos");
                Console.WriteLine("3 - Controle de Ticket's");
                Console.WriteLine("4 - Controle de Patio");
                Console.WriteLine("5 - Controle Financeiro");
                Console.WriteLine("6 - Sair");
                Console.WriteLine("-------------------------------");
                Console.WriteLine();

                var opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        MenuClientes();
                        break;
                    case 2:
                        MenuVeiculos();
                        break;
                    case 3:
                        MenuTickets();
                        break;
                    case 4:
                        MenuPatio();
                        break;
                    case 5:
                        MenuFinanceiro();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("\nInsira um Valor Valido!...");
                        Thread.Sleep(1000);
                        MenuPrincipal();
                        break;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                MenuPrincipal();
            }
            
        }
        
        static void MenuClientes()
        {
            Console.Clear();
            Console.WriteLine("-----Controle de Clientes-----");
            Console.WriteLine("1 - Cadastrar Clientes");
            Console.WriteLine("2 - Listar Clientes");
            Console.WriteLine("3 - Voltar");
            Console.WriteLine("4 - Sair");
            Console.WriteLine("------------------------------");
            Console.WriteLine();

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    CadastroDeCliente();
                    break;
                case 2:
                    ListarClientes();
                    break;
                case 3:
                    MenuPrincipal();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInsira um Valor Valido!...");
                    Thread.Sleep(1000);
                    MenuClientes();
                    break;
            }
        }

        static void MenuVeiculos()
        {
            Console.Clear();
            Console.WriteLine("-----Controle de Veiculos-----");
            Console.WriteLine("1 - Cadastrar Veiculos");
            Console.WriteLine("2 - Listar Veiculos");
            Console.WriteLine("3 - Voltar");
            Console.WriteLine("4 - Sair");
            Console.WriteLine("------------------------------");
            Console.WriteLine();

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    CadastroDeVeiculo();
                    break;
                case 2:
                    ListarVeiculos();
                    break;
                case 3:
                    MenuPrincipal();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInsira um Valor Valido!...");
                    Thread.Sleep(1000);
                    MenuVeiculos();
                    break;
            }
        }

        static void MenuTickets()
        {
            Console.Clear();
            Console.WriteLine("-------Controle de Ticket's-------");
            Console.WriteLine("1 - Cadastrar Ticket");
            Console.WriteLine("2 - Finalizar Ticket");
            Console.WriteLine("3 - Listar Ticket's Ativos");
            Console.WriteLine("4 - Listar Ticket's Finalizados");
            Console.WriteLine("5 - Voltar");
            Console.WriteLine("6 - Sair");
            Console.WriteLine("----------------------------------");
            Console.WriteLine();

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    CadastroDeTicket();
                    break;
                case 2:
                    FechamentoDeTicket();
                    break;
                case 3:
                    ListarTicketsAtivos();
                    break;
                case 4:
                    ListarTicketsFinalizados();
                    break;
                case 5:
                    MenuPrincipal();
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInsira um Valor Valido!...");
                    Thread.Sleep(1000);
                    MenuTickets();
                    break;
            }
        }

        static void MenuFinanceiro()
        {
            Console.Clear();
            Console.WriteLine("-----CONTROLE FINANCEIRO-----");
            Console.WriteLine("1 - Selecionar Período");
            Console.WriteLine("2 - Voltar");
            Console.WriteLine("3 - Sair");
            Console.WriteLine($"Valor arrecadado hoje: { servico.ValorPorPeriodo(DateTime.Today, DateTime.Now).ToString("F2") }");
            Console.WriteLine("-----------------------------");
            Console.WriteLine();

            var opcao = int.Parse(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    ValorPorPeriodo();
                    break;
                case 2:
                    MenuPrincipal();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInsira um Valor Valido!...");
                    Thread.Sleep(1000);
                    MenuFinanceiro();
                    break;
            }
        }

        static void MenuPatio()
        {
            Console.Clear();
            Patio patio = servico.PatioEstacionamento();

            Console.WriteLine("-------------------------Controle de Patio-------------------------");
            Console.WriteLine($"Capacidade Total: {patio.Capacidade_Total} - " +
                $"Vagas Ocupadas: { patio.Vagas_Ocupadas } - " +
                $"Vagas Disponiveis: { patio.Vagas_Disponiveis}");
            Console.WriteLine("-------------------------------------------------------------------");

            string listaEstacionamento = servico.ListaEstacionamento();

            Console.WriteLine(listaEstacionamento);

            Console.Write("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuPrincipal();
        }

        static void CadastroDeCliente()
        {
            Console.Clear();
            Console.WriteLine("Insira o nome do Cliente: ");
            var nome = Console.ReadLine();

            Console.WriteLine("Insira o CPF do Cliente");
            var cpf = Console.ReadLine();

            servico.CadastrarCliente(nome, cpf);

            Console.Clear();
            MenuClientes();       
        }

        static void ListarClientes()
        {
            Console.Clear();

            string clientes = servico.ListagemClientes();


            Console.Write(clientes);

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuClientes();
        }

        static void CadastroDeVeiculo()
        {
            Console.Clear();
            Console.WriteLine("Insira a Placa do Veiculo:");
            var placa = Console.ReadLine();

            Console.WriteLine("Insira a Marca do Veiculo:");
            var marca = Console.ReadLine();

            Console.WriteLine("Insira o Modelo do Veiculo:");
            var modelo = Console.ReadLine();

            Console.WriteLine("Cliente Passante ou Fixo?: P/F");
            string tipoCliente = Console.ReadLine();

            while (tipoCliente != "P" && tipoCliente != "F")
            {
                Console.WriteLine("Insira um valor valido!:");
                tipoCliente = Console.ReadLine();
            }

            string dono_Id = null;
            string nome = null;

            if (tipoCliente == "F") {
                Console.WriteLine("Insira a Identificação do Dono do Veiculo:");
                dono_Id = Console.ReadLine();
            }
            else if (tipoCliente == "P")
            {
                Console.WriteLine("Insira o Nome do Dono do Veiculo:");
                nome = Console.ReadLine();
            }
            
            servico.CadastrarVeiculo(placa, marca, modelo, tipoCliente, dono_Id, nome);

            Console.Clear();
            MenuVeiculos();
        }
    
        static void ListarVeiculos()
        {
            Console.Clear();

            string veiculos = servico.ListagemVeiculos();

            Console.Write(veiculos);
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuVeiculos();
        }

        static void ValorPorPeriodo()
        {
            Console.Clear();
            Console.WriteLine("Informe a Data Inicial: ");
            DateTime dataInicial = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Informe a Data Final: ");
            DateTime dataFinal = DateTime.Parse(Console.ReadLine());

            double valor = servico.ValorPorPeriodo(dataInicial, dataFinal);

            Console.Clear();
            Console.WriteLine($"Valor Arrecadado no Período Informado: {valor} " );
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuFinanceiro();
        }

        static void CadastroDeTicket()
        {
            Console.Clear();
            Console.WriteLine("Insira a Placa do Veiculo: ");
            var placa = Console.ReadLine();

            servico.CadastrarTicket(placa);

            Console.Clear();
            MenuTickets();
        }

        static void FechamentoDeTicket()
        {
            Console.Clear();
            Console.WriteLine("Insira o Numero do Ticket: ");
            var id_Ticket = Console.ReadLine();

            servico.FecharTicket(id_Ticket);

            Console.Clear();
            MenuTickets();
        }

        static void ListarTicketsAtivos()
        {
            Console.Clear();

            string tickets = servico.ListagemTicketsAtivos();

            Console.Write(tickets);
            
            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuTickets();
        }

        static void ListarTicketsFinalizados()
        {
            Console.Clear();

            string tickets = servico.ListagemTicketsFinalizados();

            Console.Write(tickets);

            Console.WriteLine("Pressione qualquer tecla para voltar...");
            Console.ReadLine();

            MenuTickets();
        }

        
    }
}
