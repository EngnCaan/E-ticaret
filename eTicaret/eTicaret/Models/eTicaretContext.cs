using System.Collections.Generic;
using System.Data.Entity;

namespace eTicaret.Models
{
    public class ETicaretContext : DbContext
    {
        // Base constructor çağrısı: "ETicaretContext" connection string'ine göre
        public ETicaretContext() : base("ETicaretContext")
        {
        }

        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }
        public DbSet<OdemeBilgisi> OdemeBilgileri { get; set; }
    }
}
