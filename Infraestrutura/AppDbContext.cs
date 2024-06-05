using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Infraestrutura
{
    public class AppDbContext : DbContext
    {
        /* --- Entidades ---  */

        // Usuario
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estatisticas> Estatisticas { get; set; }
        public DbSet<InformHome> InformHome { get; set; }

        // Rede
        public DbSet<Post> Posts { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }

        /* --- Interações entre IDs ---  */

        public DbSet<Curtida> Curtidas { get; set; }
        public DbSet<Amizade> Amizades { get; set; }

        /* --- Teste do Banco ---*/

        public DbSet<TestTable> TestTable { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ApolloDatabase.sqlite");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            base.OnConfiguring(optionsBuilder);

        }
    }
}
