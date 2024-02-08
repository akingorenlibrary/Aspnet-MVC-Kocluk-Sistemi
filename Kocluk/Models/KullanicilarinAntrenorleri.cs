using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class KullanicilarinAntrenorleri
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }


        [ForeignKey("Antrenor")]
        public int AntrenorId { get; set; }
        public Kullanici Antrenor { get; set; }
    }
}
