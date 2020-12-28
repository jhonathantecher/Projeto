using Projeto.Entity;
using Projeto.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Projeto.Service
{
    public class Servico
    {
        #region "Diretórios e Lists"
        string diretorioCliente = @"C:\Users\jhonathan.rowinski\Desktop\Cliente.txt";
        string diretorioVeiculo = @"C:\Users\jhonathan.rowinski\Desktop\Veiculo.txt";
        string diretorioTicket = @"C:\Users\jhonathan.rowinski\Desktop\Ticket.txt";
        string diretorioPatio = @"C:\Users\jhonathan.rowinski\Desktop\Patio.txt";

        List<Cliente> ListClientes = new List<Cliente>();
        List<Veiculo> ListVeiculos = new List<Veiculo>();
        List<Ticket> ListTickets = new List<Ticket>();
        List<Patio> ListPatio = new List<Patio>();
        #endregion

        public Servico()
        {
            if (!File.Exists(diretorioCliente))
                using (File.Create(diretorioCliente)) { }

            if (!File.Exists(diretorioVeiculo))
                using (File.Create(diretorioVeiculo)) { }

            if (!File.Exists(diretorioTicket))
                using (File.CreateText(diretorioTicket)) { }

            if (!File.Exists(diretorioPatio))
                using (StreamWriter sw = File.CreateText(diretorioPatio))
                {
                    sw.WriteLine("1,50,0");
                }

            CarregarLists();
        }

        private void CarregarLists()
        {
            //Cliente
            using (StreamReader sr = new StreamReader(diretorioCliente))
            {
                while (!sr.EndOfStream)
                {
                    var dados = sr.ReadLine().Split(",");

                    ListClientes.Add(new Cliente
                    {
                        Id = dados[0],
                        Nome = dados[1],
                        TipoCliente = dados[2] == "Fixo" ? TipoCliente.Fixo : TipoCliente.Passante
                    });
                }
            }

            //Veiculo
            using (StreamReader sr = new StreamReader(diretorioVeiculo))
            {
                while (!sr.EndOfStream)
                {
                    var dados = sr.ReadLine().Split(",");

                    ListVeiculos.Add(new Veiculo
                    {
                        Id = dados[0],
                        Id_Dono = dados[1],
                        Marca = dados[2],
                        Modelo = dados[3]
                    });
                }
            }

            //Ticket
            using (StreamReader sr = new StreamReader(diretorioTicket))
            {
                while (!sr.EndOfStream)
                {
                    var dados = sr.ReadLine().Split(",");
                    DateTime? saida;

                    if (dados[3] != "")
                        saida = DateTime.Parse(dados[3]);
                    else
                        saida = null;

                    ListTickets.Add(new Ticket
                    {
                        Id = dados[0],
                        Id_Veiculo = dados[1],
                        DataEntrada = DateTime.Parse(dados[2]),
                        DataSaida = saida,
                        Valor = Double.Parse(dados[4], CultureInfo.InvariantCulture)
                    });
                }
            }

            //Patio
            using (StreamReader sr = new StreamReader(diretorioPatio))
            {
                while (!sr.EndOfStream)
                {
                    var dados = sr.ReadLine().Split(",");

                    ListPatio.Add(new Patio
                    {
                        Id = int.Parse(dados[0]),
                        Capacidade_Total = int.Parse(dados[1]),
                        Vagas_Ocupadas = int.Parse(dados[2]),
                    });
                }
            }
        }

        #region "Cliente"
        public void CadastrarCliente(string nome, string cpf)
        {
            //Busca para verificar se o Cliente já existe.
            var clienteExiste = BuscarCliente(cpf);

            if (clienteExiste == null)
            {
                var cli = new Cliente(cpf, nome, TipoCliente.Fixo);

                SalvarCliente(cli);
                ListClientes.Add(cli);

                Console.WriteLine("\nCadastrado com Sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                throw new Exception("Cliente já existente!");
            }
        }

        private void SalvarCliente(Cliente cli)
        {
            using (StreamWriter sw = File.AppendText(diretorioCliente))
            {
                sw.WriteLine($"{cli.Id},{cli.Nome},{cli.TipoCliente}");
            }
        }
        #endregion

        #region "Veículo"
        public void CadastrarVeiculo(string placa, string marca, string modelo, string dono_Id, string nome, string tipoCliente)
        {
            var idCliente = "";

            //Verifica se o parametro dono_Id possui um valor.
            //Se sim, significa que o Cliente informado pelo Usuario é um ClienteFixo.
            //Se não, significa que o Cliente informado pelo Usuario é um ClientePassante.

            //Caso o Cliente seja um ClienteFixo, é necessario que ele esteja cadastrado no Cadastro de Cliente.
            //Portanto a condição tambem verifica se o Cliente informado existe.
            //Desse modo não há como informar um ClienteFixo inexistente.

            if (dono_Id != null) //Fixo
            {
                //Verifica se o ClienteFixo Existe.
                var clienteExiste = BuscarCliente(dono_Id);

                if (clienteExiste == null)
                    throw new Exception("Cliente Inexistente!");

                idCliente = dono_Id;
            }
            else //Passante
            {
                //Faz a leitura do List onde os Clientes são salvos e verifica a quantidade.
                //Desse modo ele gera um ID de acordo com a quantidade de linhas no arquivo.
                idCliente = (ListClientes.Count + 1).ToString();

                //Cria um ClientePassante e salva.
                var cli = new Cliente(idCliente, nome, TipoCliente.Passante);
                SalvarCliente(cli);
                ListClientes.Add(cli);
            }

            var vei = new Veiculo(placa, idCliente, marca, modelo);
            SalvarVeiculo(vei);
            ListVeiculos.Add(vei);

            Console.WriteLine("\nCadastrado com Sucesso!");
            Thread.Sleep(1000);
        }

        private void SalvarVeiculo(Veiculo vei)
        {
            using (StreamWriter sw = File.AppendText(diretorioVeiculo))
            {
                sw.WriteLine($"{vei.Id},{vei.Id_Dono},{vei.Marca},{vei.Modelo}");
            }
        }
        #endregion

        #region "Ticket"
        //Realiza a Operação de Inclusão de um Ticket.
        public void CadastrarTicket(string placa)
        {
            //Verifica se o Veiculo existe.
            var vei = BuscarVeiculo(placa);

            if (vei != null)
            {
                //Verifica se o Veiculo informado não esta no Patio.
                //Dessa forma um Ticket não pode ser gerado duas vezes para o mesmo Veiculo.
                if (!VerificarVeiculoEstacionado(placa))
                {
                    var pat = BuscarPatio();

                    //Verifica se há Vagas disponiveis.
                    if (pat.Vagas_Ocupadas < pat.Capacidade_Total)
                    {
                        //Se o veiculo existir e houverem Vagas Disponiveis, o Ticket é gerado.
                        var tic = new Ticket((ListTickets.Count + 1).ToString(), placa, DateTime.Now);
                        SalvarTicket(tic);
                        ListTickets.Add(tic);

                        AtualizarPatio(true);

                        Console.WriteLine("\nTicket Gerado com Sucesso!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw new Exception("Patio Lotado!");
                    }
                }
                else
                {
                    throw new Exception("O Veiculo já se encontra no Estacionamento!");
                }
            }
            else
            {
                throw new Exception("Veiculo não Encontrado!");
            }
        }

        private void SalvarTicket(Ticket tic)
        {
            using (StreamWriter sw = File.AppendText(diretorioTicket))
            {
                sw.WriteLine($"{tic.Id},{tic.Id_Veiculo},{tic.DataEntrada},{tic.DataSaida},{tic.Valor}");
            }
        }

        //Realiza a Operação de encerramento de um Ticket.
        public string FinalizarTicket(string id_Ticket)
        {
            //Busca o Ticket para Verificar se ele existe.
            var ticket = BuscarTicket(id_Ticket);

            if (ticket != null)
            {
                //Verifica se o Ticket esta ativo para poder ser finalizado.
                if (ticket.DataSaida == null)
                {
                    Console.Clear();

                    AtualizarPatio(false);

                    var novoTicket = AtualizarTicket(ticket);

                    //Busca o Cliente e o Veiculo presentes no Ticket.
                    var veiculo = BuscarVeiculo(ticket.Id_Veiculo);
                    var cliente = BuscarCliente(veiculo.Id_Dono);

                    //Retorna uma String com o Ticket atualizado.
                    return $"-----Ticket Finalizado com Sucesso!-----\n" +
                            $"Ticket: {novoTicket.Id} \n" +
                            $"Veiculo: {veiculo.Id}  - {veiculo.Marca} - {veiculo.Modelo} \n" +
                            $"Dono: {cliente.Id} - {cliente.Nome} \n" +
                            $"Entrada: {novoTicket.DataEntrada} \n" +
                            $"Saida: {novoTicket.DataSaida} \n" +
                            $"Valor: {novoTicket.Valor} \n\n";
                }
                else
                {
                    throw new Exception("Ticket Inativo!");
                }
            }
            else
            {
                throw new Exception("Ticket não Encontrado!");
            }
        }

        //Atualiza o Ticket e retorna o Ticket Atualizado.
        private Ticket AtualizarTicket(Ticket ticket)
        {
            var novoTicket = new Ticket(ticket.Id, ticket.Id_Veiculo, ticket.DataEntrada, ticket.DataSaida, ticket.Valor);

            //Armazena no Ticket a DataSaida.
            novoTicket.DataSaida = DateTime.Now;

            //Calcula e Armazena o Valor do Ticket.
            novoTicket.Valor = (1.50 * (Math.Ceiling(DateTime.Now.Subtract(ticket.DataEntrada).TotalMinutes / 15)));

            //Realiza a Leitura do Arquivo onde estão os dados.
            var text = File.ReadAllText(diretorioTicket);

            //Troca a linha onde esta o Ticket atualizando os dados.
            text = text.Replace($"{ticket.Id},{ticket.Id_Veiculo},{ticket.DataEntrada},{ticket.DataSaida},{ticket.Valor}",
                                $"{novoTicket.Id},{novoTicket.Id_Veiculo},{novoTicket.DataEntrada},{novoTicket.DataSaida},{novoTicket.Valor.ToString(CultureInfo.InvariantCulture)}");

            //Salva as informações novamente.
            File.WriteAllText(diretorioTicket, text);

            foreach (var item in ListTickets)
            {
                if(item == ticket)
                {
                    ticket.DataSaida = novoTicket.DataSaida;
                    ticket.Valor = novoTicket.Valor;
                }
            }
            return novoTicket;
        }

        private void AtualizarPatio(bool operacao)
        {
            var pat = BuscarPatio();
            if (operacao)
                pat.Vagas_Ocupadas = ++pat.Vagas_Ocupadas;
            else
                pat.Vagas_Ocupadas = --pat.Vagas_Ocupadas;

            //Recebe a linha do Patio atualizada.
            var text = $"{pat.Id},{pat.Capacidade_Total},{pat.Vagas_Ocupadas}";

            //Salva as informações novamente.
            File.WriteAllText(diretorioPatio, text);
            ListPatio[0].Vagas_Ocupadas = pat.Vagas_Ocupadas;
        }
        #endregion

        #region "Buscas"
        private Cliente BuscarCliente(string id_Cliente)
        {
            return ListClientes.Where(cliente => cliente.Id == id_Cliente).FirstOrDefault();
        }

        private Veiculo BuscarVeiculo(string id_Veiculo)
        {
            return ListVeiculos.Where(veiculo => veiculo.Id == id_Veiculo).FirstOrDefault();
        }

        private Ticket BuscarTicket(string id_Ticket)
        {
            return ListTickets.Where(ticket => ticket.Id == id_Ticket).FirstOrDefault();
        }

        public Patio BuscarPatio()
        {      
            return ListPatio.Where(patio => patio.Id == 1).FirstOrDefault();
        }
        #endregion

        #region "Listagens"
        //Retorna Listagem contendo todos os Clientes.
        public string ListagemClientes()
        {
            var listaClientes = $"-------------------------Clientes-------------------------\n";
            ListClientes.ForEach(cliente => listaClientes += string.Join(" - ", cliente.Id, cliente.Nome, cliente.TipoCliente) + "\n");
            return listaClientes;
        }

        //Retorna Listagem contendo todos os Veiculos.
        public string ListagemVeiculos()
        {
            var listaVeiculos = $"-------------------------Veículos-------------------------\n";
            foreach (var veiculo in ListVeiculos)
            {
                var cliente = ListClientes.Where(cliente => cliente.Id == veiculo.Id_Dono).FirstOrDefault();

                listaVeiculos += $"Placa: {veiculo.Id} \n" +
                                 $"Marca: {veiculo.Marca} \n" +
                                 $"Modelo: {veiculo.Modelo} \n" +
                                 $"Dono: {cliente.Id} - {cliente.Nome} \n\n";
            }
            return listaVeiculos;
        }

        //Retorna uma Listagem com todos os Ticket's ativos.
        public string ListagemTicketsAtivos()
        {
            var listaTickets = "--------------------Ticket's Ativos--------------------\n";
            var ticketsAtivos = ListTickets.Where(ticket => ticket.DataSaida == null).ToList();

            foreach (var ticket in ticketsAtivos)
            {
                var veiculo = ListVeiculos.Where(veiculo => veiculo.Id == ticket.Id_Veiculo).FirstOrDefault();
                var cliente = ListClientes.Where(cliente => cliente.Id == veiculo.Id_Dono).FirstOrDefault();

                listaTickets += $"Ticket: {ticket.Id} \n" +
                                $"Veiculo: {veiculo.Id}  - {veiculo.Marca} - {veiculo.Modelo} \n" +
                                $"Dono: {cliente.Id} - {cliente.Nome} \n" +
                                $"Entrada: {ticket.DataEntrada} \n\n";
            }
            return listaTickets;
        }

        //Retorna uma Listagem com todos os Ticket's que ja foram finalizados.
        public string ListagemTicketsFinalizados()
        {
            var listaTickets = "--------------------Ticket's Finalizados--------------------\n";
            var ticketsFinalizados = ListTickets.Where(ticket => ticket.DataSaida != null).ToList();

            foreach (var ticket in ticketsFinalizados)
            {
                var veiculo = ListVeiculos.Where(veiculo => veiculo.Id == ticket.Id_Veiculo).FirstOrDefault();
                var cliente = ListClientes.Where(cliente => cliente.Id == veiculo.Id_Dono).FirstOrDefault();

                listaTickets += $"Ticket: {ticket.Id} \n" +
                                $"Veiculo: {veiculo.Id}  - {veiculo.Marca} - {veiculo.Modelo} \n" +
                                $"Dono: {cliente.Id} - {cliente.Nome} \n" +
                                $"Entrada: {ticket.DataEntrada} \n" +
                                $"Saída: {ticket.DataSaida} \n" +
                                $"Valor: {ticket.Valor} \n\n";
            }
            return listaTickets;
        }

        //Retorna uma Listagem dos veículos que estao no estacionamento.
        public string ListagemEstacionamento()
        {
            var listaEstacionamento = "";
            var ticketsAtivos = ListTickets.Where(ticket => ticket.DataSaida == null).ToList();

            foreach (var ticket in ticketsAtivos)
            {
                var veiculo = ListVeiculos.Where(veiculo => veiculo.Id == ticket.Id_Veiculo).FirstOrDefault();
                var cliente = ListClientes.Where(cliente => cliente.Id == veiculo.Id_Dono).FirstOrDefault();

                listaEstacionamento += $"Veiculo: {veiculo.Id}  - {veiculo.Marca} - {veiculo.Modelo} \n" +
                                       $"Dono: {cliente.Id} - {cliente.Nome} \n\n";
            }
            return listaEstacionamento;
        }
        #endregion

        #region "Financeiro"
        //Calcula o valor total arrecadado nos Ticket's em determinado Período.
        public double ValorPorPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            return ListTickets
                .Where(ticket => ticket.DataSaida != null && ticket.DataEntrada >= dataInicial && ticket.DataSaida <= dataFinal)
                .Sum(ticket => ticket.Valor);
        }
        #endregion

        #region "Validações"
        public bool ValidacaoCPF(string cpf)
        {
            if (cpf.Length == 11 && long.TryParse(cpf, out long numero))
                return true;
            return false;
        }

        public bool ValidacaoTipoCliente(string tipoCliente)
        {
            if (tipoCliente.Equals("P") || tipoCliente.Equals("F"))
                return true;
            return false;
        }

        public bool ValidacaoPlaca(string placa)
        {
            Regex regex = new Regex(@"^[A-Z]{3}\d{4}$");

            if (regex.IsMatch(placa))
                return true;
            return false;
        }

        public bool ValidacaoDateTime(string date)
        {
            if (DateTime.TryParse(date, out DateTime result))
                return true;
            return false;
        }

        public bool ValidacaoDateTimeFinal(string date, DateTime dataInicial)
        {
            if (DateTime.TryParse(date, out DateTime result))
                if (DateTime.Parse(date) > dataInicial)
                    return true;
                else
                    Console.WriteLine("\nA Data Final deve ser maior que a Data Inicial!");
            return false;
        }

        public void VerificarVeiculoExiste(string placa)
        {
            var vei = BuscarVeiculo(placa);
            if (vei != null)
                throw new Exception("Veiculo ja Existente!");
        }

        //Verifica se um determinado Veiculo se encontra no estacionamento.
        private bool VerificarVeiculoEstacionado(string placa)
        {
            var veiculo = ListTickets.Where(ticket => ticket.DataSaida == null && ticket.Id_Veiculo == placa).ToList();

            if (veiculo.Count > 0)
                return true;
            return false;
        }
        #endregion
    }
}
