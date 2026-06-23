using ControleTeste.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleTeste.Context.Configuration;

// classe de configuração para a entidade 
public class AlteracaoConfiguration : IEntityTypeConfiguration<Alteracao>
{
    public void Configure(EntityTypeBuilder<Alteracao> builder)
    { 

        builder
            .ToTable("tb_alteracao");

        builder
            .HasKey(a => a.AlteracaoId);

        builder.Property(a => a.NumeroAlteracao)
            .IsRequired();

        builder.Property(a => a.Titulo)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(a => a.Descricao)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(a => a.Tipo)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.Sistema)
            .IsRequired();

        builder.Property(a => a.DataAbertura)
           .IsRequired();

        builder.Property(a => a.MenuSistema)
           .HasMaxLength(100)
           .IsRequired();

        builder.Property(a => a.Observacao)
           .HasMaxLength(2000);

    }       
}