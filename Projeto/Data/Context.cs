using Microsoft.EntityFrameworkCore;
using Projeto.Entity;

namespace Projeto.Data
{
    public class Context : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Password=sa1234567%;Persist Security Info=True;User ID=sa;Database=Estacionamento;Data Source=T24\\ESTACIONAMENTO");
        }
    }
}
