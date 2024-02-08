using System.ComponentModel.DataAnnotations;

namespace Kocluk.Dto
{
    public class MesajDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Boş bırakmayın")]
        public string Mesaj { get; set; }
    }
}
