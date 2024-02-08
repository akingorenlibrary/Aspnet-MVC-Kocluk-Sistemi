using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class DanisanBeslenmeProgrami
    {
        [Key]
        public int Id { get; set; }
        public string Hedef { get; set; }
        public string GunlukOgunler { get; set; }
        public string KaloriHedefi { get; set; }

        [ForeignKey("Kullanici")]
        public int DanisanId { get; set; }
        public Kullanici Kullanici { get; set; }

        [ForeignKey("Antrenor")]
        public int AntrenorId { get; set; }
        public Kullanici Antrenor { get; set; }
    }
}
