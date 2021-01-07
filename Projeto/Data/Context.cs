using Microsoft.EntityFrameworkCore;
using Projeto.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto.Data
{
    public class Context : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;userid=developer;password=1234567;database=Estacionamento");
            //developer@127.0.0.1:3306
        }
    }
}
