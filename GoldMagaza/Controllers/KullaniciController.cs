using GoldMagaza.Models;
using System.Linq;
using System.Web.Mvc;

namespace GoldMagaza.Controllers
{
    public class KullaniciController : Controller
    {
        private VeritabaniContext db = new VeritabaniContext();

        // GET: /Kullanici/Giris
        public ActionResult Giris()
        {
            return View();
        }

        // POST: /Kullanici/Giris
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Giris(string eposta, string sifre)
        {
            if (string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(sifre))
            {
                ViewBag.Hata = "Lütfen tüm alanları doldurun.";
                return View();
            }

            var kullanici = db.Kullanicilar.FirstOrDefault(k => k.Eposta == eposta && k.Sifre == sifre);

            if (kullanici != null)
            {
                Session["kullanici"] = kullanici;

                if (kullanici.Rol == "Yonetici")
                    return RedirectToAction("Index", "Yonetim");
                else
                    return RedirectToAction("Index", "Anasayfa");
            }

            ViewBag.Hata = "E-posta veya şifre hatalı.";
            return View();
        }

        // GET: /Kullanici/Kayit
        public ActionResult Kayit()
        {
            return View(new Kullanici());
        }

        // POST: /Kullanici/Kayit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kayit(Kullanici yeniKullanici)
        {
            // 🛠️ Önemli: ModelState kontrolünden önce Rol atanmalı
            yeniKullanici.Rol = "Kullanici";

            if (!ModelState.IsValid)
                return View(yeniKullanici);

            if (db.Kullanicilar.Any(k => k.Eposta == yeniKullanici.Eposta))
            {
                ModelState.AddModelError("Eposta", "Bu e-posta adresi zaten kayıtlı.");
                return View(yeniKullanici);
            }

            db.Kullanicilar.Add(yeniKullanici);
            db.SaveChanges();

            TempData["KayitBasarili"] = "Kayıt başarılı. Lütfen giriş yapın.";
            return RedirectToAction("Giris");
        }

        // GET: /Kullanici/Cikis
        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Giris");
        }
        // GET: /Kullanici/Profil
        public ActionResult Profil()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris");

            var oturumKullanici = (Kullanici)Session["kullanici"];
            var kullanici = db.Kullanicilar.Find(oturumKullanici.Id);
            return View(kullanici);
        }

        // POST: /Kullanici/Profil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profil(Kullanici guncellenen)
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris");

            var mevcut = (Kullanici)Session["kullanici"];
            var dbKullanici = db.Kullanicilar.Find(mevcut.Id);

            if (dbKullanici == null)
                return RedirectToAction("Giris");

            // Rol tekrar set edilmezse ModelState geçersiz olur!
            guncellenen.Rol = dbKullanici.Rol;

            if (!ModelState.IsValid)
                return View(guncellenen); // Hatalı giriş varsa geri göster

            // Değerleri güncelle
            dbKullanici.AdSoyad = guncellenen.AdSoyad;
            dbKullanici.Eposta = guncellenen.Eposta;
            dbKullanici.Sifre = guncellenen.Sifre;
            dbKullanici.Telefon = guncellenen.Telefon;
            dbKullanici.Sehir = guncellenen.Sehir;
            dbKullanici.Adres = guncellenen.Adres;

            db.SaveChanges();

            Session["kullanici"] = dbKullanici;
            ViewBag.Basarili = "Bilgiler başarıyla güncellendi.";

            return View(dbKullanici);
        }



    }
}
