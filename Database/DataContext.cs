using Microsoft.EntityFrameworkCore;
using Agenda.Models;
using Coberturas.Models;
using Noticias.Models;
using Cadastro.Models;
using CoberturasImagens.Models;

namespace database.Data
{
    public class DataContext : DbContext
    {
        public DbSet<NoticiasModel> Noticias { get; set; } = null!;
        public DbSet<AgendaModel> Agenda { get; set; } = null!;
        public DbSet<CoberturaModel> Coberturas { get; set; } = null!;
        public DbSet<CadastroModel> Cadastro { get; set; } = null!;
        public DbSet<CoberturaImagemModel> CoberturaImagens { get; set; } = null!;


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgendaModel>(entity =>
            {
                entity.ToTable("agenda");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                      .HasColumnName("nome")
                      .IsRequired();

                entity.Property(e => e.Data)
                      .HasColumnName("data")
                      .HasColumnType("timestamp with time zone")
                      .IsRequired();

                entity.Property(e => e.Descricao)
                      .HasColumnName("descricao")
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Url)
                      .HasColumnName("url")
                      .IsRequired(false);

                entity.Property(e => e.Slug)
                      .HasColumnName("slug")
                      .HasMaxLength(255)
                      .IsRequired(false);
            });

            modelBuilder.Entity<CoberturaModel>(entity =>
            {
                entity.ToTable("cobertura");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Titulo)
                      .HasColumnName("titulo")
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Descricao)
                      .HasColumnName("descricao")
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.Local)
                      .HasColumnName("local")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Fotografo)
                      .HasColumnName("fotografo")
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Slug)
                      .HasColumnName("slug")
                      .HasMaxLength(255)
                      .IsRequired(false);

                entity.Property(e => e.Data)
                      .HasColumnName("data")
                      .HasColumnType("timestamp with time zone")
                      .IsRequired();
            });

            modelBuilder.Entity<CoberturaImagemModel>(entity =>
              {
                  entity.ToTable("cobertura_imagem");

                  entity.HasKey(e => e.Id);

                  entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                  entity.Property(e => e.Titulo)
                      .HasColumnName("titulo")
                      .IsRequired(false);

                  entity.Property(e => e.Url)
                      .HasColumnName("url")
                      .IsRequired(false);


                  entity.Property(e => e.CoberturaId)
                      .HasColumnName("cobertura_id")
                      .IsRequired();

                  entity.HasOne(e => e.Cobertura)
                      .WithMany(c => c.Imagens)    // Relação 1-N
                      .HasForeignKey(e => e.CoberturaId)
                      .OnDelete(DeleteBehavior.Cascade); // Exclui as imagens ao deletar a cobertura
              });


            modelBuilder.Entity<NoticiasModel>(entity =>
            {
                entity.ToTable("noticias");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Titulo)
                      .HasColumnName("titulo")
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Descricao)
                      .HasColumnName("descricao")
                      .IsRequired();

                entity.Property(e => e.Url)
                      .HasColumnName("url")
                      .IsRequired(false);


                entity.Property(e => e.Slug)
                      .HasColumnName("slug")
                      .HasMaxLength(255)
                      .IsRequired(false);

                entity.Property(e => e.Data)
                      .HasColumnName("data")
                      .HasColumnType("timestamp with time zone")
                      .IsRequired();
            });

            modelBuilder.Entity<CadastroModel>(entity =>
            {
                entity.ToTable("cadastro");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                      .HasColumnName("nome")
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Email)
                      .HasColumnName("email")
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.Senha)
                      .HasColumnName("senha")
                      .IsRequired()
                      .HasMaxLength(150);


            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
