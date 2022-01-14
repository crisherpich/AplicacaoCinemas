using AplicacaoCinemas.WebApi.Dominio;
using Microsoft.EntityFrameworkCore;

namespace AplicacaoCinemas.WebApi.Infraestrutura
{
    public class CinemasDbContext : DbContext
    {
        public CinemasDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Filme>().ToTable("Filmes");
            modelBuilder.Entity<Filme>().HasKey(c => c.Id);
            modelBuilder.Entity<Filme>().Property(c => c.Titulo).HasColumnType("varchar(50)");
            modelBuilder.Entity<Filme>().Property(c => c.Sinopse).HasColumnType("varchar(max)");
            modelBuilder.Entity<Filme>().Property(c => c.Duracao).HasColumnType("int");

            modelBuilder.Entity<Sessao>().ToTable("Sessoes");
            modelBuilder.Entity<Sessao>().HasKey(a => a.Id);
            modelBuilder.Entity<Sessao>().Property(a => a.Dia).HasColumnType("date");
            modelBuilder.Entity<Sessao>().Property(a => a.HoraInicio).HasColumnType("int");
            modelBuilder.Entity<Sessao>().Property(a => a.MinutoInicio).HasColumnType("int");
            modelBuilder.Entity<Sessao>().Property(a => a.QuantidadeLugares).HasColumnType("int");
            modelBuilder.Entity<Sessao>().Property(a => a.QuantidadeLugaresLivres).HasColumnType("int");
            modelBuilder.Entity<Sessao>().Property(a => a.Preco).HasColumnType("float");
            modelBuilder.Entity<Sessao>().Property(a => a.IdFilme).HasColumnType("uniqueidentifier");
            modelBuilder.Entity<Sessao>().Property("_hashConcorrencia").
                HasColumnName("token_concorrencia").
                HasConversion<string>().IsConcurrencyToken();
        }
    }
}