using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Entities;

namespace PropostaService.Infrastructure.Data;

public class PropostaDbContext : DbContext
{
    public PropostaDbContext(DbContextOptions<PropostaDbContext> options) : base(options)
    {
    }

    public DbSet<Proposta> Propostas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Proposta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeguradoNome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.SeguradoCpf).IsRequired().HasMaxLength(11);
            entity.Property(e => e.ValorPremio).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.DataCriacao).IsRequired();
        });
    }
}
