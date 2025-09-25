using Contratos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contratos.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<Person> Users => Set<Person>();
        public DbSet<Supplier> Empresas => Set<Supplier>();
        public DbSet<Contract> Contratos => Set<Contract>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Person>(e =>
            {
                e.ToTable("Persons");
                e.HasKey(x => x.Id);
                e.Property(x => x.Nome).HasMaxLength(120).IsRequired();
                e.Property(x => x.Email).HasMaxLength(180).IsRequired();
                e.Property(x => x.PasswordHash).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();
            });

            b.Entity<Supplier>(e =>
            {
                e.ToTable("Suppliers");
                e.HasKey(x => x.Id);
                e.Property(x => x.Nome).HasMaxLength(160).IsRequired();
                e.Property(x => x.Cnpj).HasMaxLength(20).IsRequired();
                e.Property(x => x.Email).HasMaxLength(180).IsRequired();
                e.Property(x => x.Telefone).HasMaxLength(40).IsRequired();
            });

            b.Entity<Contract>(e =>
            {
                e.ToTable("Contracts");
                e.HasKey(x => x.Id);
                e.Property(x => x.Status).HasConversion<int>().IsRequired();
                e.Property(x => x.Numero).IsRequired();
                e.Property(x => x.Descricao);
                e.Property(x => x.Valor);
                e.HasOne(x => x.Supplier)
                    .WithMany()
                    .HasForeignKey(x => x.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
