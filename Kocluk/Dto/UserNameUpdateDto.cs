using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class UserNameUpdateDto
    {
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz")]
        [MaxLength(100, ErrorMessage = "Ad alanı maksimum 100 karakter olmalı")]
        public string Name { get; set; }
    }
}
