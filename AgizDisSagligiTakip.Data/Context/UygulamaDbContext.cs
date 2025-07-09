using Microsoft.EntityFrameworkCore;
using AgizDisSagligiTakip.Core.Entities;

namespace AgizDisSagligiTakip.Data.Context
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {
        }

        // DbSet'ler (Tablolar)
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Hedef> Hedefler { get; set; }
        public DbSet<HedefKaydi> HedefKayitlari { get; set; }
        public DbSet<Not> Notlar { get; set; }
        public DbSet<Oneri> Oneriler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tablolar arası ilişkiler
            modelBuilder.Entity<Hedef>()
                .HasOne(h => h.Kullanici)
                .WithMany(k => k.Hedefler)
                .HasForeignKey(h => h.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HedefKaydi>()
                .HasOne(hk => hk.Hedef)
                .WithMany(h => h.HedefKayitlari)
                .HasForeignKey(hk => hk.HedefId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Not>()
                .HasOne(n => n.Kullanici)
                .WithMany(k => k.Notlar)
                .HasForeignKey(n => n.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}