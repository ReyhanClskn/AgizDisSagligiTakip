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

            // Varsayılan öneriler ekle
            modelBuilder.Entity<Oneri>().HasData(
                new Oneri { Id = 1, Metin = "Günde en az 2 kez diş fırçalamayı unutmayın!", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 2, Metin = "Diş ipi kullanmak, diş fırçasının ulaşamadığı yerleri temizler.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 3, Metin = "Şekerli içecekleri sınırlandırın, dişleriniz size teşekkür edecek.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 4, Metin = "6 ayda bir diş hekimi kontrolü yaptırmayı ihmal etmeyin.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 5, Metin = "Ağız gargarası kullanarak ağız hijyeninizi destekleyin.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 6, Metin = "Diş fırçanızı 3 ayda bir değiştirin.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 7, Metin = "Sert fırçalama yerine nazik dairesel hareketler yapın.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) },
                new Oneri { Id = 8, Metin = "Yemek sonrası 30 dakika bekleyip diş fırçalayın.", Aktif = true, OlusturmaTarihi = new DateTime(2024, 1, 1) }
            );
        }
    }
}