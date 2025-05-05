using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace GoldMagaza.Models
{
    public class VeritabaniContext : DbContext
    {
        public VeritabaniContext() : base("GoldMagazaConn")
        {
        }

        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Urun> Urunler { get; set; } 
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
        public DbSet<OdemeBilgisi> OdemeBilgileri { get; set; }

    }
}
