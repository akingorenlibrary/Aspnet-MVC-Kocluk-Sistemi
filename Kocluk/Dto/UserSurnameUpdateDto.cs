using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class UserSurnameUpdateDto
    {
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Soyadı alanı maksimum 100 karakter olmalı")]
        public string Surname { get; set; }
    }
}
