using System.ComponentModel.DataAnnotations;

namespace eTicaret.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100)]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz.")]
        [StringLength(100)]
        public string Eposta { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(50)]
        public string Sifre { get; set; }


        [StringLength(20)]
        public string Rol { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        [StringLength(20)]
        public string Telefon { get; set; }

        [StringLength(50)]
        public string Sehir { get; set; }

        [StringLength(250)]
        public string Adres { get; set; }
    }
}
