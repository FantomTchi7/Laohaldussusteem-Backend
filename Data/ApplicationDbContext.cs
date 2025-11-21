using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Arve> Arved { get; set; }
        public DbSet<Toode> Tooted { get; set; }
        public DbSet<Ettevõte> Ettevõtted { get; set; }
        public DbSet<Koostaja> Koostajad { get; set; }
        public DbSet<Tellija> Tellijad { get; set; }
        public DbSet<Administraator> Administraatorid { get; set; }
        public DbSet<Kasutaja> Kasutajad { get; set; }
        public DbSet<Produkt> Produktid { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kasutaja>()
                .HasDiscriminator(k => k.KasutajaTüüp)
                .HasValue<Koostaja>(KasutajaTüüp.Koostaja)
                .HasValue<Tellija>(KasutajaTüüp.Tellija)
                .HasValue<Administraator>(KasutajaTüüp.Administraator);

            modelBuilder.Entity<Produkt>()
                .HasDiscriminator<string>("ProduktTüüp")
                .HasValue<Produkt>("BaasProdukt")
                .HasValue<Toode>("ArveToode");

            modelBuilder.Entity<Arve>(entity =>
            {
                entity.HasMany(a => a.Tooted)
                    .WithOne(t => t.Arve)
                    .HasForeignKey(t => t.ArveId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Koostaja)
                    .WithMany()
                    .HasForeignKey(a => a.KoostajaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Tellija)
                    .WithMany()
                    .HasForeignKey(a => a.TellijaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Ettevõte)
                    .WithMany()
                    .HasForeignKey(a => a.EttevõteId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}