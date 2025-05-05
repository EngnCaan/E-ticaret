using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoldMagaza.Models
{
    public class SiparisDetay
    {
        public int Id { get; set; }

        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }

        public int UrunId { get; set; }
        public virtual Urun Urun { get; set; }

        public int Adet { get; set; }

        public decimal Fiyat { get; set; }
    }
}
