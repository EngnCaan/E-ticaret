using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldMagaza.Models
{
    public class Siparis
    {
        public int Id { get; set; }

        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }

        public DateTime Tarih { get; set; }

        public decimal ToplamTutar { get; set; }

        public virtual ICollection<SiparisDetay> SiparisDetaylar { get; set; }
    }
}
