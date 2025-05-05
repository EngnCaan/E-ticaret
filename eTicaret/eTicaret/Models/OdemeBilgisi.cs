using eTicaret.Models;
using System.ComponentModel.DataAnnotations;

namespace eTicaret.Models
{
    public class OdemeBilgisi
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Kart numarası geçersiz.")]
        [Display(Name = "Kart Numarası")]
        public string KartNumarasi { get; set; }

        [Required]
        [Display(Name = "Son Kullanma Tarihi (AA/YY)")]
        public string SonKullanmaTarihi { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        public bool Kaydet { get; set; }

        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
