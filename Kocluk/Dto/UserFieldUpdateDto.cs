using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class UserFieldUpdateDto
    {
        [Required(ErrorMessage = "Alanı boş bırakılamaz")]
        public string bosAlan { get; set; }

        public string bosAlanAdi { get; set; }
    }
}
