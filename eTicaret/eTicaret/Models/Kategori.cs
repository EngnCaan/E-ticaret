using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eTicaret.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Ad { get; set; }

        public virtual ICollection<Urun> Urunler { get; set; }
    }
}
