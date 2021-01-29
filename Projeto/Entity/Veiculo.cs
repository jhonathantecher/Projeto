namespace Projeto.Entity
{
    public class Veiculo
    {
        public string VeiculoId { get; set; }
        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public bool Excluido { get; set; }
    }
}
