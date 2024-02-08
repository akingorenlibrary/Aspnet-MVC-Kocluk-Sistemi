using Kocluk.Models;
using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class KullaniciLoginRequestDto
    {
        [Required(ErrorMessage = "Eposta alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Email formatı hatalı")]
        [MaxLength(100, ErrorMessage = "Eposta alanı maksimum 100 karakter olmalı")]
        public string Eposta { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Şifre alanı maksimum 100 karakter olmalı")]
        [MinLength(10, ErrorMessage = "Şifre minimum 10 karakter olmalı")]
        public string Sifre { get; set; }
    }
}
