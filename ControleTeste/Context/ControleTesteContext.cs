using ControleTeste.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleTeste.Context
{
    public class ControleTesteContext : DbContext
    {
        public ControleTesteContext(DbContextOptions<ControleTesteContext> options)
            : base(options)
        {
        }

        public DbSet<Alteracao> Alteracoes { get; set; }
        public DbSet<Models.Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControleTesteContext).Assembly);
        }
    }
}
