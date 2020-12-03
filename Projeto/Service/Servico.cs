using Projeto.Entity;
using Projeto.Entity.Enum;
using System.Threading;
using System;
using System.Text.RegularExpressions;

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
            //Busca para verificar se o Cliente já existe.
            var cli = BuscarCliente(cpf);

            if (cli == null)
            {
                cli = new Cliente(cpf, nome, TipoCliente.Fixo);
                cliente.ListaCliente.Add(cli);

                Console.WriteLine("\nCadastrado com Sucesso!");
                Thread.Sleep(1000);
            }
            else //Caso cli não seja nulo, significa que o Cliente já possui um cadastro.
            {
                Console.WriteLine("\nCliente já existente!");
                Thread.Sleep(1000);
            }

        }

        //Retorna Listagem contendo todo os Clientes.
        public string ListagemClientes()
        {
            var listaCli = "";

            foreach (var cliente in cliente.ListaCliente)
            {
                listaCli += cliente.Id + " - " + cliente.Nome + " - " + cliente.TipoCliente + "\n";

            }
            return listaCli;
        }

        public void CadastrarVeiculo(string placa, string marca, string modelo, string tipoCliente, string dono_Id, string nome)
        {
            Cliente cli;

            //Verifica se o parametro dono_Id possui um valor.
            //Se sim, significa que o Cliente informado pelo Usuario é um ClienteFixo e a condição cai no else.
            //Se não, significa que o Cliente informado pelo Usuario é um ClientePassante.

            //Caso o Cliente seja um ClienteFixo, é necessario que ele esteja cadastrado no Cadastro de Cliente.
            //Portanto a condição else verifica se o Cliente informado existe na ListaCliente.
            //Desse modo não há como informar um ClienteFixo inexistente.

            if (dono_Id == null) //Passante
            {
                //Cria um ClientePassante e adiciona ele na ListaCliente.
                cli = new Cliente((cliente.ListaCliente.Count + 1).ToString(), nome, TipoCliente.Passante);
                cliente.ListaCliente.Add(cli);
            }
            else //Fixo
            {
                //Verifica se o ClienteFixo Existe.
                cli = BuscarCliente(dono_Id);

                if (cli == null)
                {
                    Console.WriteLine("\nCliente Inexistente!");
                    Thread.Sleep(1000);
                    return;
                }
            }

            //Cadastra o Veiculo - Insere na Lista
            var vei = new Veiculo(placa, cli.Id, marca, modelo);
            veiculo.ListaVeiculos.Add(vei);

            Console.WriteLine("\nCadastrado com Sucesso!");
            Thread.Sleep(1000);
        }

        public bool ValidacaoCPF(string cpf)
        {
            if (cpf.Length == 11 && long.TryParse(cpf, out long numero))
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

        public bool ValidacaoTipoCliente(string tipoCliente)
        {
            if (tipoCliente == "P" || tipoCliente == "F")
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

        public bool VerificarVeiculoExiste(string placa)
        {
            var vei = BuscarVeiculo(placa);

            if (vei != null)
                return true;
            return false;
        }

        //Retorna Listagem contendo todos os Veiculos.
        public string ListagemVeiculos()
        {
            string ListVei = "";

            foreach (var veiculo in veiculo.ListaVeiculos)
            {
                var dono = BuscarCliente(veiculo.Id_Dono);

                ListVei += $"Placa: {veiculo.Id_Placa} \n" +
                    $"Marca: {veiculo.Marca} \n" +
                    $"Modelo: {veiculo.Modelo} \n" +
                    $"Dono:  {dono.Id}  -  {dono.Nome}" +
                    $"\n\n";
            }

            return ListVei;
        }

        //Realiza a Operação de Inclusão de um Ticket.
        public void CadastrarTicket(string placa)
        {
            //Verifica se o Veiculo existe.
            var vei = BuscarVeiculo(placa);

            //Se o Veiculo existir ele sera diferente de null.
            if (vei != null)
            {
                //Verifica se o Veiculo informado não esta no Patio.
                //Dessa forma um Ticket não pode ser gerado duas vezes para o mesmo Veiculo.
                if (VerificarVeiculoEstacionado(placa) == false)
                {
                    //Verifica se há Vagas disponiveis.
                    if (patio.Vagas_Ocupadas < patio.Capacidade_Total)
                    {
                        //Se o veiculo existir e houverem Vagas Disponiveis, o Ticket é gerado.
                        var tic = new Ticket(
                            (ticket.ListaTickets.Count + 1).ToString(),
                            vei.Id_Placa,
                            DateTime.Now);

                        ticket.ListaTickets.Add(tic);
                        patio.Vagas_Ocupadas++;

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
                    Console.WriteLine("\nO Veiculo já se encontra no Estacionamento!");
                    Thread.Sleep(1500);
                }
            }
            else
            {
                Console.WriteLine("\nVeiculo não Encontrado!");
                Thread.Sleep(1000);
            }
        }

        //Realiza a Operação de encerramento de um Ticket.
        public string FecharTicket(string id_Ticket)
        {
            //Busca o Ticket para Verificar se ele existe.
            //Caso exista, irá retornar o Index do Ticket na Lista Ticket para que ele possa ser localizado e finalizado.
            int index = BuscarIndexTicket(id_Ticket);

            if (index >= 0) //Se o Ticket existir o index será igual ou superior a zero.
            {
                if (ticket.ListaTickets[index].DataSaida == null) //Verifica se o Ticket esta ativo para poder ser finalizado.
                {
                    Console.Clear();

                    //Calcula quanto tempo o Veiculo ficou no estacionamento e armazena na variavel 'duracao'.
                    TimeSpan duracao = DateTime.Now.Subtract(ticket.ListaTickets[index].DataEntrada);

                    //Armazena no Ticket a DataSaida.
                    ticket.ListaTickets[index].DataSaida = DateTime.Now;

                    //Calcula o Valor do Ticket.
                    ticket.ListaTickets[index].Valor = 1.50 * (Math.Ceiling(duracao.TotalMinutes / 15));

                    //Atualiza as Vagas no Patio.
                    patio.Vagas_Ocupadas--;

                    //Busca o Cliente e o Veiculo presentes no Ticket.
                    var tic = BuscarTicket(id_Ticket);
                    var vei = BuscarVeiculo(tic.Id_Veiculo);
                    var cli = BuscarCliente(vei.Id_Dono);

                    //Retorna uma String com o Ticket atualizado.
                    return $"-----Ticket Finalizado com Sucesso!-----\n" +
                        $"Ticket: {tic.Id_Ticket} \n" +
                        $"Veiculo: {tic.Id_Veiculo}  - {vei.Marca} - {vei.Modelo} \n" +
                        $"Dono: {cli.Id} - {cli.Nome} \n" +
                        $"Entrada: {tic.DataEntrada} \n" +
                        $"Saida: {tic.DataSaida} \n" +
                        $"Valor: { tic.Valor} \n\n";
                }
                else
                {
                    return "\nTicket Inativo!";
                }
            }
            else
            {
                return "\nTicket não Encontrado!\n";
            }
        }

        //Retorna uma Listagem com todos os Ticket's que estão ativos, quando o veiculo ainda esta no estacionamento.
        public string ListagemTicketsAtivos()
        {
            string listaTic = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida == null) //Se a DataSaida é igual null, significa que o Ticket ainda esta ativo.
                {
                    var vei = BuscarVeiculo(ticket.Id_Veiculo);
                    var cli = BuscarCliente(vei.Id_Dono);

                    listaTic += $"Ticket: {ticket.Id_Ticket} \n" +
                        $"Veiculo: {ticket.Id_Veiculo}  - {vei.Marca} - {vei.Modelo} \n" +
                        $"Dono: {cli.Id} - {cli.Nome} \n" +
                        $"Entrada: {ticket.DataEntrada} \n" +
                        $"Saida: {ticket.DataSaida} \n\n";
                }
            }
            return listaTic;
        }

        //Retorna uma Listagem com todos os Ticket's que ja foram finalizados.
        public string ListagemTicketsFinalizados()
        {
            string listaTic = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida != null) //Se a DataSaida é diferente de null, significa que o Ticket foi finalizado.
                {
                    var vei = BuscarVeiculo(ticket.Id_Veiculo);
                    var cli = BuscarCliente(vei.Id_Dono);

                    listaTic += $"Ticket: {ticket.Id_Ticket} \n" +
                        $"Veiculo: {ticket.Id_Veiculo}  - {vei.Marca} - {vei.Modelo} \n" +
                        $"Dono: {cli.Id} - {cli.Nome} \n" +
                        $"Entrada: {ticket.DataEntrada} \n" +
                        $"Saida: {ticket.DataSaida} \n" +
                        $"Valor: {ticket.Valor} \n\n";
                }
            }
            return listaTic;
        }

        //Busca um Veiculo pelo ID e retorna.
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

        //Busca um Cliente pelo ID e retorna.
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

        //Busca um Ticket pelo ID e retorna.
        private Ticket BuscarTicket(string id_Ticket)
        {
            for (int i = 0; i <= ticket.ListaTickets.Count - 1; i++)
            {
                if (ticket.ListaTickets[i].Id_Ticket == id_Ticket)
                {
                    return ticket.ListaTickets[i];
                }
            }
            return null;
        }

        //Busca um Ticket na Lista e retorna o Index dele na Lista.
        //Utilizado para realizar alterações no Ticket dentro da Lista Ticket, por isso é necessário obter o Index.
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

        //Retorna Listagem contendo todos os Veiculos presentes no Estacionamento.
        public string ListaEstacionamento()
        {
            var listaEstacionamento = "";

            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida == null)
                {
                    var vei = BuscarVeiculo(ticket.Id_Veiculo);
                    var cli = BuscarCliente(vei.Id_Dono);

                    listaEstacionamento += $"Veiculo: {ticket.Id_Veiculo} - {vei.Marca} - {vei.Modelo} \nDono: {cli.Id} - {cli.Nome} \n\n";
                }
            }
            return listaEstacionamento;
        }

        //Verifica se um determinado Veiculo se encontra no estacionamento.
        public bool VerificarVeiculoEstacionado(string placa)
        {
            foreach (var ticket in ticket.ListaTickets)
            {
                if (ticket.DataSaida == null && ticket.Id_Veiculo == placa)
                {
                    return true;
                }
            }
            return false;
        }

        //Calcula o valor total arrecadado nos Ticket's em determinado Período.
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
