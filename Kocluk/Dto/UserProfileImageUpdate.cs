using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class UserProfileImageUpdate
    {
        [Required(ErrorMessage = "Profil fotoğrafı alanı boş bırakılamaz")]
        public IFormFile ProfilFotografDosyasi { get; set; }
    }
}
