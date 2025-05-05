using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GoldMagaza.Models;

namespace GoldMagaza.Controllers
{
    public class AnasayfaController : Controller
    {
        private VeritabaniContext db = new VeritabaniContext();

        public ActionResult Index()
        {
            var urunler = db.Urunler.Include("Kategori").ToList();
            return View(urunler);
        }

        public ActionResult Detay(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var urun = db.Urunler.Include("Kategori").FirstOrDefault(u => u.Id == id);
            if (urun == null)
                return HttpNotFound();

            return View(urun);
        }

        public ActionResult SepeteEkle(int id)
        {
            var urun = db.Urunler.Find(id);
            if (urun == null)
                return HttpNotFound();

            var sepet = Session["sepet"] as List<Urun> ?? new List<Urun>();
            sepet.Add(urun);
            Session["sepet"] = sepet;

            TempData["Mesaj"] = "Ürün sepete eklendi!";
            return RedirectToAction("Index");
        }

        public ActionResult Sepet()
        {
            var sepet = Session["sepet"] as List<Urun> ?? new List<Urun>();
            return View(sepet);
        }

        public ActionResult SepetiTemizle()
        {
            Session["sepet"] = null;
            TempData["Mesaj"] = "Sepet temizlendi.";
            return RedirectToAction("Sepet");
        }

        public ActionResult SiparisVer()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            var kullanici = (Kullanici)Session["kullanici"];
            var sepet = Session["sepet"] as List<Urun>;

            if (sepet == null || !sepet.Any())
                return RedirectToAction("Sepet");

            var siparis = new Siparis
            {
                KullaniciId = kullanici.Id,
                Tarih = DateTime.Now,
                ToplamTutar = sepet.Sum(u => u.Fiyat),
                SiparisDetaylar = new List<SiparisDetay>()
            };

            foreach (var urun in sepet)
            {
                siparis.SiparisDetaylar.Add(new SiparisDetay
                {
                    UrunId = urun.Id,
                    Adet = 1,
                    Fiyat = urun.Fiyat
                });
            }

            db.Siparisler.Add(siparis);
            db.SaveChanges();

            Session["sepet"] = null;
            TempData["Mesaj"] = "Siparişiniz başarıyla oluşturuldu.";
            return RedirectToAction("OdemeTamamlandi");
        }

        public ActionResult Siparislerim()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            var kullanici = (Kullanici)Session["kullanici"];

            var siparisler = db.Siparisler
                .Where(s => s.KullaniciId == kullanici.Id)
                .OrderByDescending(s => s.Tarih)
                .ToList();

            return View(siparisler);
        }

        public ActionResult Kategoriler()
        {
            var kategoriler = db.Kategoriler.ToList();
            return View(kategoriler);
        }

        public ActionResult KategoriyeGore(int id)
        {
            var urunler = db.Urunler
                            .Include("Kategori")
                            .Where(u => u.KategoriId == id)
                            .ToList();

            ViewBag.Kategori = db.Kategoriler.Find(id)?.Ad ?? "Kategori";
            return View(urunler);
        }

        public ActionResult Odeme()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            return View(new OdemeBilgisi());
        }

        [HttpPost]
        public ActionResult Odeme(OdemeBilgisi model)
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            if (!ModelState.IsValid)
                return View(model);

            var kullanici = (Kullanici)Session["kullanici"];
            model.KullaniciId = kullanici.Id;

            if (model.Kaydet)
            {
                db.OdemeBilgileri.Add(model);
                db.SaveChanges();
            }

            return RedirectToAction("SiparisVer");
        }

        public ActionResult SepetiOnayla()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            return RedirectToAction("BilgiGir", "Odeme");
        }

        public ActionResult OdemeTamamlandi()
        {
            return View();
        }
    }
}
