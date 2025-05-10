using System.Linq;
using System.Web.Mvc;
using GoldMagaza.Models;

namespace GoldMagaza.Controllers
{
    public class YonetimController : Controller
    {
        private VeritabaniContext db = new VeritabaniContext();

        // Admin paneline giriş
        public ActionResult Index()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");
            return View();
        }

        // Ürün ekleme (GET)
        public ActionResult UrunEkle()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            ViewBag.Kategoriler = new SelectList(db.Kategoriler.ToList(), "Id", "Ad");
            return View();
        }

        // Ürün ekleme (POST)
        [HttpPost]
        public ActionResult UrunEkle(Urun urun)
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            if (ModelState.IsValid)
            {
                db.Urunler.Add(urun);
                db.SaveChanges();
                ViewBag.Basari = "Ürün başarıyla eklendi!";
            }

            ViewBag.Kategoriler = new SelectList(db.Kategoriler.ToList(), "Id", "Ad", urun.KategoriId);
            return View(urun);
        }

        // Ürün listeleme
        public ActionResult UrunListele()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            var urunler = db.Urunler.Include("Kategori").ToList();
            return View(urunler);
        }

        // Ürün silme
        public ActionResult UrunSil(int id)
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            var urun = db.Urunler.Find(id);
            if (urun != null)
            {
                db.Urunler.Remove(urun);
                db.SaveChanges();
            }

            return RedirectToAction("UrunListele");
        }

        // Ürün düzenleme (GET)
        public ActionResult UrunDuzenle(int id)
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            var urun = db.Urunler.Find(id);
            if (urun == null) return HttpNotFound();

            ViewBag.Kategoriler = new SelectList(db.Kategoriler.ToList(), "Id", "Ad", urun.KategoriId);
            return View(urun);
        }

        // Ürün düzenleme (POST)
        [HttpPost]
        public ActionResult UrunDuzenle(Urun urun)
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            if (ModelState.IsValid)
            {
                db.Entry(urun).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UrunListele");
            }

            ViewBag.Kategoriler = new SelectList(db.Kategoriler.ToList(), "Id", "Ad", urun.KategoriId);
            return View(urun);
        }

        // Kategori ekleme (GET)
        public ActionResult KategoriEkle()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");
            return View();
        }

        // Kategori ekleme (POST)
        [HttpPost]
        public ActionResult KategoriEkle(Kategori kategori)
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            if (ModelState.IsValid)
            {
                db.Kategoriler.Add(kategori);
                db.SaveChanges();
                ViewBag.Basari = "Kategori başarıyla eklendi.";
            }

            return View(kategori);
        }

        // Kategori listeleme
        public ActionResult KategoriListele()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            var kategoriler = db.Kategoriler.ToList();
            return View(kategoriler);
        }

        // Tüm siparişleri listeleme (kargo bilgisi yok)
        public ActionResult TumSiparisler()
        {
            if (!YoneticiMi()) return RedirectToAction("Giris", "Kullanici");

            var siparisler = db.Siparisler
                               .OrderByDescending(s => s.Tarih)
                               .ToList();

            return View(siparisler);
        }

        // Yardımcı: yönetici kontrolü
        private bool YoneticiMi()
        {
            var kullanici = Session["kullanici"] as Kullanici;
            return kullanici != null && kullanici.Rol == "Yonetici";
        }
        // GET: /Yonetim/KategoriSil/5
        public ActionResult KategoriSil(int id)
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            var kullanici = (Kullanici)Session["kullanici"];
            if (kullanici.Rol != "Yonetici")
                return RedirectToAction("YetkisizErisim", "Hata");

            var kategori = db.Kategoriler.Find(id);

            if (kategori != null)
            {
                // Önce bu kategoriye bağlı ürünleri kontrol et, varsa engelle
                bool urunVar = db.Urunler.Any(u => u.KategoriId == id);
                if (urunVar)
                {
                    TempData["Hata"] = "Bu kategoriye bağlı ürünler var, önce ürünleri silin.";
                }
                else
                {
                    db.Kategoriler.Remove(kategori);
                    db.SaveChanges();
                    TempData["Basari"] = "Kategori başarıyla silindi.";
                }
            }

            return RedirectToAction("KategoriListele");
        }

    }
}
