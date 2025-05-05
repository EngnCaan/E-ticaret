using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GoldMagaza.Models
{
    public class Urun
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Ad { get; set; }

        public decimal Fiyat { get; set; }

        public string ResimYolu { get; set; }

        [ForeignKey("Kategori")]
        public int KategoriId { get; set; }

        public virtual Kategori Kategori { get; set; }
    }
}
