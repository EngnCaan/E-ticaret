namespace eTicaret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IlkOlusturma : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uruns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ad = c.String(nullable: false, maxLength: 150),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ResimYolu = c.String(),
                        KategoriId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kategoris", t => t.KategoriId, cascadeDelete: true)
                .Index(t => t.KategoriId);
            
            CreateTable(
                "dbo.Kullanicis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AdSoyad = c.String(nullable: false, maxLength: 100),
                        Eposta = c.String(nullable: false, maxLength: 100),
                        Sifre = c.String(nullable: false, maxLength: 50),
                        Rol = c.String(maxLength: 20),
                        Telefon = c.String(maxLength: 20),
                        Sehir = c.String(maxLength: 50),
                        Adres = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OdemeBilgisis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KartNumarasi = c.String(nullable: false, maxLength: 16),
                        SonKullanmaTarihi = c.String(nullable: false),
                        CVV = c.String(nullable: false, maxLength: 4),
                        Kaydet = c.Boolean(nullable: false),
                        KullaniciId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kullanicis", t => t.KullaniciId, cascadeDelete: true)
                .Index(t => t.KullaniciId);
            
            CreateTable(
                "dbo.SiparisDetays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiparisId = c.Int(nullable: false),
                        UrunId = c.Int(nullable: false),
                        Adet = c.Int(nullable: false),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Siparis", t => t.SiparisId, cascadeDelete: true)
                .ForeignKey("dbo.Uruns", t => t.UrunId, cascadeDelete: true)
                .Index(t => t.SiparisId)
                .Index(t => t.UrunId);
            
            CreateTable(
                "dbo.Siparis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KullaniciId = c.Int(nullable: false),
                        Tarih = c.DateTime(nullable: false),
                        ToplamTutar = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kullanicis", t => t.KullaniciId, cascadeDelete: true)
                .Index(t => t.KullaniciId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiparisDetays", "UrunId", "dbo.Uruns");
            DropForeignKey("dbo.SiparisDetays", "SiparisId", "dbo.Siparis");
            DropForeignKey("dbo.Siparis", "KullaniciId", "dbo.Kullanicis");
            DropForeignKey("dbo.OdemeBilgisis", "KullaniciId", "dbo.Kullanicis");
            DropForeignKey("dbo.Uruns", "KategoriId", "dbo.Kategoris");
            DropIndex("dbo.Siparis", new[] { "KullaniciId" });
            DropIndex("dbo.SiparisDetays", new[] { "UrunId" });
            DropIndex("dbo.SiparisDetays", new[] { "SiparisId" });
            DropIndex("dbo.OdemeBilgisis", new[] { "KullaniciId" });
            DropIndex("dbo.Uruns", new[] { "KategoriId" });
            DropTable("dbo.Siparis");
            DropTable("dbo.SiparisDetays");
            DropTable("dbo.OdemeBilgisis");
            DropTable("dbo.Kullanicis");
            DropTable("dbo.Uruns");
            DropTable("dbo.Kategoris");
        }
    }
}
