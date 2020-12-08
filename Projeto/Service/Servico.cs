using Projeto.Entity;
using Projeto.Entity.Enum;
using System.Threading;
using System;
using System.Text.RegularExpressions;
using System.IO;

namespace Projeto.Service
{
    public class Servico
    { 
        string diretorioCliente = @"C:\Users\jhonathan.rowinski\Desktop\Cliente.txt";
        string diretorioVeiculo = @"C:\Users\jhonathan.rowinski\Desktop\Veiculo.txt";
        string diretorioTicket = @"C:\Users\jhonathan.rowinski\Desktop\Ticket.txt";
        string diretorioPatio = @"C:\Users\jhonathan.rowinski\Desktop\Patio.txt";

        public bool ValidacaoCPF(string cpf)
        {
            if (cpf.Length == 11 && long.TryParse(cpf, out long numero))
                return true;
            return false;
        }

        public bool ValidacaoTipoCliente(string tipoCliente)
        {
            if (tipoCliente == "P" || tipoCliente == "F")
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

        public void CadastrarCliente(string nome, string cpf)
        {
            //Busca para verificar se o Cliente já existe.
            var clienteExiste = BuscarCliente(cpf);

            if (clienteExiste == "")
            {
                var cli = new Cliente(cpf, nome, TipoCliente.Fixo);

                SalvarCliente(cli);

                Console.WriteLine("\nCadastrado com Sucesso!");
                Thread.Sleep(1000);
            }
            else
            {
                //Caso cli não seja nulo, significa que o Cliente já possui um cadastro.
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

        //Retorna Listagem contendo todos os Clientes.
        public string ListagemClientes()
        {
            var listaCli = "";

            using (FileStream fs = new FileStream(diretorioCliente, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linha = sr.ReadLine().Split(",");
                        listaCli += $"{linha[0]} - {linha[1]} - {linha[2]}\n";
                    }
                }
            }
            return listaCli;
        }

        public void VerificarVeiculoExiste(string placa)
        {
            var vei = BuscarVeiculo(placa);

            if (vei != "")
                throw new Exception("\nVeiculo ja Existente!");

        }
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

                if (clienteExiste == "")
                    throw new Exception("Cliente Inexistente!");
                idCliente = dono_Id;
            }
            else //Passante
            {
                //Faz a leitura do arquivo onde os Clientes são salvos e verifica quantas linhas possui.
                //Desse modo ele gera um ID de acordo com a quantidade de linhas no arquivo.
                idCliente = (QuantidadeLinhasTXT(diretorioCliente) + 1).ToString();

                //Cria um ClientePassante e salva.
                var cli = new Cliente(idCliente, nome, TipoCliente.Passante);
                SalvarCliente(cli);
            }

            var vei = new Veiculo(placa, idCliente, marca, modelo);
            SalvarVeiculo(vei);

            Console.WriteLine("\nCadastrado com Sucesso!");
            Thread.Sleep(1000);
        }

        //Retorna a Quantidade de Linhas que o TXT possui.
        private int QuantidadeLinhasTXT(string diretorio)
        {
            var count = 0;
            using (FileStream fs = new FileStream(diretorio, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        sr.ReadLine();
                        count++;                       
                    }
                }
            }
            return count;
        }

        private void SalvarVeiculo(Veiculo vei)
        {
            using (StreamWriter sw = File.AppendText(diretorioVeiculo))
            {
                sw.WriteLine($"{vei.Id},{vei.Id_Dono},{vei.Marca},{vei.Modelo}");
            }
        }

        //Retorna Listagem contendo todos os Veiculos.
        public string ListagemVeiculos()
        {
            var listaVei = "";

            using (FileStream fs = new FileStream(diretorioVeiculo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linha = sr.ReadLine().Split(",");

                        var cli = BuscarCliente(linha[1]);
                        var linhaDono = cli.Split(",");

                        listaVei += $"Placa: {linha[0]} \n" +
                            $"Marca: {linha[2]} \n" +
                            $"Modelo: {linha[3]} \n" +
                            $"Dono:  {linhaDono[0]}  -  {linhaDono[1]}" +
                            $"\n\n";
                    }
                }
            }
            return listaVei;
        }      

        //Realiza a Operação de Inclusão de um Ticket.
        public void CadastrarTicket(string placa)
        {
            //Verifica se o Veiculo existe.
            var vei = BuscarVeiculo(placa);

            if (vei != "")
            {
                //Verifica se o Veiculo informado não esta no Patio.
                //Dessa forma um Ticket não pode ser gerado duas vezes para o mesmo Veiculo.
                if (VerificarVeiculoEstacionado(placa) == false)
                {
                    var pat = BuscarPatio();
                    var itemPatio = pat.Split(",");
                    //Verifica se há Vagas disponiveis.
                    if (int.Parse(itemPatio[2]) < int.Parse(itemPatio[1]))
                    {
                        //Se o veiculo existir e houverem Vagas Disponiveis, o Ticket é gerado.
                        var tic = new Ticket((QuantidadeLinhasTXT(diretorioTicket) + 1).ToString(), placa, DateTime.Now);

                        itemPatio[2] = (int.Parse(itemPatio[2]) + 1).ToString();

                        //Realiza a Leitura do Arquivo onde estão os dados.
                        string text = File.ReadAllText(diretorioPatio);
                        //Troca a linha onde esta o Patio atualizando os dados.
                        text = text.Replace(pat, $"{itemPatio[0]},{itemPatio[1]},{itemPatio[2]}");
                        //Salva as informações novamente.
                        File.WriteAllText(diretorioPatio, text);

                        SalvarTicket(tic);

                        Console.WriteLine("\nTicket Gerado com Sucesso!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw new Exception("\nPatio Lotado!");
                    }
                }
                else
                {
                    throw new Exception("\nO Veiculo já se encontra no Estacionamento!");
                }
            }
            else
            {
                throw new Exception("\nVeiculo não Encontrado!");
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
        public string FecharTicket(string id_Ticket)
        {
            //Busca o Ticket para Verificar se ele existe.
            var tic = BuscarTicket(id_Ticket);

            if (tic != "")
            {
                //Se o Ticket existir, armazena seus dados no Vetor.
                //Os itens[] virão listados na mesma ordem de cadastro.
                //Portanto itemTicket[0] por exemplo é o ID do Ticket, e assim por diante. 
                var itemTicket = tic.Split(",");

                //Item Ticket[3] é a DataSaida.
                if (itemTicket[3] == "") //Verifica se o Ticket esta ativo para poder ser finalizado.
                {
                    Console.Clear();

                    //Calcula quanto tempo o Veiculo ficou no estacionamento e armazena na variavel 'duracao'.
                    TimeSpan duracao = DateTime.Now.Subtract(DateTime.Parse(itemTicket[2]));

                    //Armazena no Ticket a DataSaida.
                    itemTicket[3] = DateTime.Now.ToString();

                    //Calcula o Valor do Ticket.
                    itemTicket[4] = (1.50 * (Math.Ceiling(duracao.TotalMinutes / 15))).ToString().Replace(".","").Replace(",",".");

                    //Atualiza as Vagas no Patio.
                    var pat = BuscarPatio();
                    var itemPatio = pat.Split(",");

                    itemPatio[2] = (int.Parse(itemPatio[2]) - 1).ToString();

                    string text = File.ReadAllText(diretorioPatio);
                    text = text.Replace(pat, $"{itemPatio[0]},{itemPatio[1]},{itemPatio[2]}");
                    File.WriteAllText(diretorioPatio, text);
                    
                    //Realiza a Leitura do Arquivo onde estão os dados.
                    text = File.ReadAllText(diretorioTicket);
                   
                    //Troca a linha onde esta o Ticket atualizando os dados.
                    text = text.Replace(tic, $"{itemTicket[0]},{itemTicket[1]},{itemTicket[2]},{itemTicket[3]},{itemTicket[4]}");
                    
                    //Salva as informações novamente.
                    File.WriteAllText(diretorioTicket, text);

                    //Busca o Cliente e o Veiculo presentes no Ticket.
                    var vei = BuscarVeiculo(itemTicket[1]);
                    var itemVeiculo = vei.Split(",");

                    var cli = BuscarCliente(itemVeiculo[1]);
                    var itemCliente = cli.Split(",");

                    //Retorna uma String com o Ticket atualizado.
                    return $"-----Ticket Finalizado com Sucesso!-----\n" +
                        $"Ticket: {itemTicket[0]} \n" +
                        $"Veiculo: {itemVeiculo[0]}  - {itemVeiculo[2]} - {itemVeiculo[3]} \n" +
                        $"Dono: {itemCliente[0]} - {itemCliente[1]} \n" +
                        $"Entrada: {itemTicket[2]} \n" +
                        $"Saida: {itemTicket[3]} \n" +
                        $"Valor: {itemTicket[4]} \n\n";
                }
                else
                {
                    throw new Exception("\nTicket Inativo!");
                }
            }
            else
            {
                throw new Exception("\nTicket não Encontrado!\n");
            }
        }

        //Retorna uma Listagem com todos os Ticket's que estão ativos, quando o veiculo ainda esta no estacionamento.
        public string ListagemTicketsAtivos()
        {
            var listaTic = "";

            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linhaTic = sr.ReadLine().Split(",");

                        if (linhaTic[3] == "")
                        {
                            var vei = BuscarVeiculo(linhaTic[1]);
                            var linhaVei = vei.Split(",");

                            var cli = BuscarCliente(linhaVei[1]);
                            var linhaCli = cli.Split(",");

                            listaTic += $"Ticket: {linhaTic[0]} \n" +
                                  $"Veiculo: {linhaVei[0]}  - {linhaVei[2]} - {linhaVei[3]} \n" +
                                  $"Dono: {linhaCli[0]} - {linhaCli[1]} \n" +
                                  $"Entrada: {linhaTic[2]} \n" +
                                  $"\n\n";
                        }            
                    }
                }
            }
            return listaTic;
        }

        //Retorna uma Listagem com todos os Ticket's que ja foram finalizados.
        public string ListagemTicketsFinalizados()
        {
            var listaTic = "";

            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linhaTic = sr.ReadLine().Split(",");

                        if (linhaTic[3] != "")
                        {
                            var vei = BuscarVeiculo(linhaTic[1]);
                            var linhaVei = vei.Split(",");

                            var cli = BuscarCliente(linhaVei[1]);
                            var linhaCli = cli.Split(",");

                            listaTic += $"Ticket: {linhaTic[0]} \n" +
                                  $"Veiculo: {linhaVei[0]}  - {linhaVei[2]} - {linhaVei[3]} \n" +
                                  $"Dono: {linhaCli[0]} - {linhaCli[1]} \n" +
                                  $"Entrada: {linhaTic[2]} \n" +
                                  $"Saída: {linhaTic[3]} \n" +
                                  $"Valor: {linhaTic[4]} \n" +
                                  $"\n\n";
                        }
                    }
                }
            }
            return listaTic;
        }

        //Busca um Cliente pelo ID e retorna a String correspondente a Linha do Cliente.
        private string BuscarCliente(string id_Cliente)
        {
            var cli = "";

            using (FileStream fs = new FileStream(diretorioCliente, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    bool encontrado = false;

                    while (!sr.EndOfStream && encontrado == false)
                    {
                        var linha = sr.ReadLine().Split(",");

                        if (linha[0] == id_Cliente)
                        {
                            cli = $"{linha[0]},{linha[1]},{linha[2]}";
                            encontrado = true;
                        }
                    }
                }
            }
            return cli;
        }

        //Busca um Veiculo pelo ID e retorna a String correspondente a Linha do Veiculo.
        private string BuscarVeiculo(string id_Veiculo)
        {
            var vei = "";

            using (FileStream fs = new FileStream(diretorioVeiculo, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    bool encontrado = false;

                    while (!sr.EndOfStream && encontrado == false)
                    {
                        var linha = sr.ReadLine().Split(",");

                        if (linha[0] == id_Veiculo)
                        {
                            vei = $"{linha[0]},{linha[1]},{linha[2]},{linha[3]}";
                            encontrado = true;
                        }
                    }
                }
            }
            return vei;
        }

        //Busca um Ticket pelo ID e retorna a String correspondente a Linha do Ticket.
        private string BuscarTicket(string id_Ticket)
        {
            var tic = "";

            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    bool encontrado = false;

                    while (!sr.EndOfStream && encontrado == false)
                    {
                        var linha = sr.ReadLine().Split(",");

                        if (linha[0] == id_Ticket)
                        {
                            tic = $"{linha[0]},{linha[1]},{linha[2]},{linha[3]},{linha[4]}";
                            encontrado = true;
                        }
                    }
                }
            }
            return tic;
        }

        public string BuscarPatio()
        {
            var pat = "";

            using (FileStream fs = new FileStream(diretorioPatio, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linha = sr.ReadLine().Split(",");

                        if (linha[0] == "1")
                        {
                            pat = $"{linha[0]},{linha[1]},{linha[2]}";
                        }
                    }
                }
            }
            return pat;
        }

        //Retorna Listagem contendo todos os Veiculos presentes no Estacionamento.
        public string ListaEstacionamento()
        {
            var listaEstacionamento = "";

            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {                   
                    while (!sr.EndOfStream)
                    {
                        var itemTicket = sr.ReadLine().Split(",");

                        var vei = BuscarVeiculo(itemTicket[1]);
                        var itemVeiculo = vei.Split(",");

                        var cli = BuscarCliente(itemVeiculo[1]);
                        var itemCliente = cli.Split(",");

                        listaEstacionamento += $"Veiculo: {itemVeiculo[0]} - " +
                        $"{itemVeiculo[2]} - {itemVeiculo[3]} \n" +
                        $"Dono: {itemCliente[0]} - {itemCliente[1]} \n\n";
                    }
                }
            }
            return listaEstacionamento;
        }

        //Verifica se um determinado Veiculo se encontra no estacionamento.
        private bool VerificarVeiculoEstacionado(string placa)
        {
            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var linha = sr.ReadLine().Split(",");

                        if (linha[1] == placa && linha[3] == "")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;     
        }

        //Calcula o valor total arrecadado nos Ticket's em determinado Período.
        public double ValorPorPeriodo(DateTime dataInicial, DateTime dataFinal)
        {
            double total = 0.00;

            using (FileStream fs = new FileStream(diretorioTicket, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        var itemTicket = sr.ReadLine().Split(",");

                        if (itemTicket[3] != "")
                        {
                            if (DateTime.Parse(itemTicket[3]) >= dataInicial && DateTime.Parse(itemTicket[3]) <= dataFinal)
                            {
                                total += Double.Parse(itemTicket[4].Replace(".",","));
                            }
                        }                  
                    }
                }
            }
            return total;
        }
    }
}
