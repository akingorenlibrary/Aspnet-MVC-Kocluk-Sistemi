using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class IlerlemeDurumu
    {
        [Key]
        public int Id { get; set; }
        public int Kilo { get; set; }
        public int Boy { get; set; }
        public int VucutYagOrani { get; set; }
        public int KasKutlesi { get; set; }
        public int VucutKitleIndeksi { get; set; }

        [ForeignKey("Kullanici")]
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
