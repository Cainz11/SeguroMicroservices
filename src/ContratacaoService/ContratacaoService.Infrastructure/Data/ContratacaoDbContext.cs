using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Infrastructure.Data;

public class ContratacaoDbContext : DbContext
{
    public ContratacaoDbContext(DbContextOptions<ContratacaoDbContext> options) : base(options)
    {
    }

    public DbSet<Contratacao> Contratacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contratacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PropostaId).IsRequired();
            entity.Property(e => e.DataContratacao).IsRequired();
            entity.HasIndex(e => e.PropostaId).IsUnique();
        });
    }
}
