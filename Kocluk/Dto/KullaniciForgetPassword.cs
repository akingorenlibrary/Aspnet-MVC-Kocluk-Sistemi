using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class KullaniciForgetPassword
    {
        public int id;

        [Required(ErrorMessage = "Eposta alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Email formatı hatalı")]
        [MaxLength(100, ErrorMessage = "Eposta alanı maksimum 100 karakter olmalı")]
        public string Eposta { get; set; }
    }
}
