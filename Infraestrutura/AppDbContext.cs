using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Infraestrutura
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }
        public DbSet<Estatisticas> Estatisticas { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Curtida> Curtidas { get; set; }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ApolloDatabase.sqlite");
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

            base.OnConfiguring(optionsBuilder);

        }
    }
}
