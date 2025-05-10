using GoldMagaza.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GoldMagaza.Controllers
{
    public class OdemeController : Controller
    {
        private VeritabaniContext db = new VeritabaniContext();

        [HttpGet]
        public ActionResult BilgiGir()
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            var kullanici = (Kullanici)Session["kullanici"];

            var mevcutOdeme = db.OdemeBilgileri.FirstOrDefault(o => o.KullaniciId == kullanici.Id);

            ViewBag.MevcutBilgi = mevcutOdeme != null ? mevcutOdeme : null;
            return View(new OdemeBilgisi());
        }

        [HttpPost]
        public ActionResult BilgiGir(OdemeBilgisi odeme, bool bilgileriKaydet)
        {
            if (Session["kullanici"] == null)
                return RedirectToAction("Giris", "Kullanici");

            var kullanici = (Kullanici)Session["kullanici"];

            if (!ModelState.IsValid)
            {
                return View(odeme);
            }

            if (bilgileriKaydet)
            {
                odeme.KullaniciId = kullanici.Id;
                db.OdemeBilgileri.Add(odeme);
                db.SaveChanges();
            }

            // Ödeme sonrası sipariş verme adımına yönlendir
            return RedirectToAction("SiparisVer", "Anasayfa");
        }
    }
}
