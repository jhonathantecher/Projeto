using Projeto.Entity;
using Projeto.Service;
using System;
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
            catch (Exception e)
            {
                Console.WriteLine($"\nErro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuPrincipal();
            }

        }

        static void MenuClientes()
        {
            try
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
                        CadastroCliente();
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
            catch (Exception e)
            {
                Console.WriteLine($"\nErro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuClientes();
            }
        }

        static void MenuVeiculos()
        {
            try
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
                        CadastroVeiculo();
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
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuPrincipal();
            }
            
        }

        static void MenuTickets()
        {
            try
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
                        CadastroTicket();
                        break;
                    case 2:
                        FechamentoTicket();
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
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuPrincipal();
            }
        }

        static void MenuFinanceiro()
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuPrincipal();
            }
            
        }

        static void MenuPatio()
        {
            try
            {
                Console.Clear();
                var patio = servico.BuscarPatio();

                Console.WriteLine("-------------------------Controle de Patio-------------------------");
                Console.WriteLine($"Capacidade Total: {patio.Capacidade_Total} - " +
                                  $"Vagas Ocupadas: {patio.Vagas_Ocupadas} - " +
                                  $"Vagas Disponiveis: {patio.Capacidade_Total - patio.Vagas_Ocupadas}");
                Console.WriteLine("-------------------------------------------------------------------");

                var listaEstacionamento = servico.listagemTicketsAtivosOuEstacionamento(false);

                Console.WriteLine(listaEstacionamento);

                Console.WriteLine("Pressione qualquer tecla para voltar...");
                Console.ReadLine();

                MenuPrincipal();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuPrincipal();
            }
            
        }

        static void CadastroCliente()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Insira o nome do Cliente: ");
                var nome = Console.ReadLine();

                Console.WriteLine("Insira o CPF do Cliente");
                var cpf = Console.ReadLine();

                //Verifica se o CPF tem 11 digitos e a variavel é numerica.
                while (servico.ValidacaoCPF(cpf) == false)
                {
                    Console.WriteLine("\nInsira um CPF valido!");
                    cpf = Console.ReadLine();
                }

                servico.CadastrarCliente(nome, cpf);

                MenuClientes();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuClientes();
            }       
        }

        static void ListarClientes()
        {
            try
            {
                Console.Clear();

                var clientes = servico.ListagemClientes();

                Console.Write(clientes);

                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuClientes();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuClientes();
            }
        }

        static void CadastroVeiculo()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Insira a Placa do Veiculo:");
                var placa = Console.ReadLine();

                //Verifica se a Placa é valida.
                while (servico.ValidacaoPlaca(placa) == false)
                {
                    Console.WriteLine("\nInsira uma Placa Valida!");
                    placa = Console.ReadLine();
                }

                servico.VerificarVeiculoExiste(placa);

                Console.WriteLine("Insira a Marca do Veiculo:");
                var marca = Console.ReadLine();

                Console.WriteLine("Insira o Modelo do Veiculo:");
                var modelo = Console.ReadLine();

                Console.WriteLine("Cliente Passante ou Fixo?: P/F");
                var tipoCliente = Console.ReadLine();

                //Força o Usuario a Inserir P ou F.
                while (servico.ValidacaoTipoCliente(tipoCliente) == false)
                {
                    Console.WriteLine("\nInsira um valor valido!");
                    tipoCliente = Console.ReadLine();
                }

                string dono_Id = null;
                string nome = null;

                if (tipoCliente == "F")
                {
                    Console.WriteLine("Insira a Identificação do Dono do Veiculo:");
                    dono_Id = Console.ReadLine();
                }
                else if (tipoCliente == "P")
                {
                    Console.WriteLine("Insira o Nome do Dono do Veiculo:");
                    nome = Console.ReadLine();
                }

                servico.CadastrarVeiculo(placa, marca, modelo, dono_Id, nome, tipoCliente);

                Console.Clear();
                MenuVeiculos();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nErro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuVeiculos();
            }
        }

        static void ListarVeiculos()
        {
            try
            {
                Console.Clear();

                var veiculos = servico.ListagemVeiculos();

                Console.Write(veiculos);
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuVeiculos();
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nErro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuVeiculos();
            }          
        }

        static void CadastroTicket()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Insira a Placa do Veiculo: ");
                var placa = Console.ReadLine();

                servico.CadastrarTicket(placa);

                Console.Clear();
                MenuTickets();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuTickets();
            }
        }

        static void FechamentoTicket()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Insira o Numero do Ticket: ");
                var id_Ticket = Console.ReadLine();

                var retorno = servico.FinalizarTicket(id_Ticket);

                Console.WriteLine(retorno);

                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuTickets();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuTickets();
            }
        }

        static void ListarTicketsAtivos()
        {
            try
            {
                Console.Clear();

                var tickets = servico.listagemTicketsAtivosOuEstacionamento(true);

                Console.Write(tickets);

                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuTickets();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuTickets();
            }        
        }

        static void ListarTicketsFinalizados()
        {
            try
            {
                Console.Clear();

                var tickets = servico.ListagemTicketsFinalizados();

                Console.Write(tickets);

                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuTickets();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuTickets();
            }         
        }

        static void ValorPorPeriodo()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Informe a Data Inicial: (DD/MM/AAAA)");
                var date = Console.ReadLine();

                //Verifica se a Data informada esta no formato correto.
                while (servico.ValidacaoDateTime(date) == false)
                {
                    Console.WriteLine("\nInsira uma Data Valida!");
                    date = Console.ReadLine();
                }
                DateTime dataInicial = DateTime.Parse(date);


                Console.WriteLine("Informe a Data Final: (DD/MM/AAAA)");
                date = Console.ReadLine();

                //Mesma logica acima, mas com o diferencial de verificar se a DataFinal é maior do que a DataInicial.
                while (servico.ValidacaoDateTimeFinal(date, dataInicial) == false)
                {
                    Console.WriteLine("\nInsira uma Data Valida!");
                    date = Console.ReadLine();
                }
                DateTime dataFinal = DateTime.Parse(date);

                //Recebe o valor total do período chamando a função.
                double valor = servico.ValorPorPeriodo(dataInicial, dataFinal);

                Console.Clear();
                Console.WriteLine($"Valor Arrecadado no Período Informado: {valor.ToString("F2")} ");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();

                MenuFinanceiro();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                Console.WriteLine("Pressione qualquer tecla para retornar...");
                Console.ReadLine();
                MenuFinanceiro();
            }
            
        }
    }
}
