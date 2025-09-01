using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;
using Desafio.Models;

namespace Desafio.Data;

public class EFCoreContext : DbContext
{
    public EFCoreContext(DbContextOptions<EFCoreContext> options) : base(options)
    {

    }

    // deixar construtor vazio para migrations
    public EFCoreContext()
    {

    }

    // Descomentar para migrações sem injeção de dependência
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         optionsBuilder.UseSqlServer("Server=localhost;Database=DesafioDB;Trusted_Connection=True;TrustServerCertificate=True;"); 
    //     }
    // }

    // Adicionar DbSets para as entidades
    public DbSet<ContaModel> Contas { get; set; }
    public DbSet<TransacaoModel> Transacoes { get; set; }
    public DbSet<PessoaModel> Pessoas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações das entidades
        modelBuilder.Entity<ContaModel>(entity =>
        {
            entity.HasKey(pk => pk.IdConta);
            entity.Property(e => e.Saldo).HasColumnType("decimal(18,2)"); // Define o tipo decimal com precisão
            entity.Property(e => e.LimiteSaqueDiario).HasColumnType("decimal(18,2)"); // Define o tipo decimal com precisão
            entity.HasOne(e => e.Pessoa).WithMany().HasForeignKey("PessoaId").IsRequired(); // Relação com PessoaModel
        });

        modelBuilder.Entity<TransacaoModel>(entity =>
        {
            entity.HasKey(pk => pk.IdTransacao);
            entity.Property(e => e.Valor).HasColumnType("decimal(18,2)"); // Define o tipo decimal com precisão
            entity.HasOne(e => e.Conta).WithMany().HasForeignKey("ContaId").IsRequired(); // Relação com ContaModel
        });

        modelBuilder.Entity<PessoaModel>(entity =>
        {
            entity.HasKey(pk => pk.IdPessoa);
            entity.HasIndex(e => e.Cpf).IsUnique(); // Garante que o CPF seja único
        });

    }
}
