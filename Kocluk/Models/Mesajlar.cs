using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class Mesajlar
    {
        public int Id { get; set; }


        [ForeignKey("Alici")]
        public int AliciId { get; set; }
        public Kullanici Alici { get; set; }


        [ForeignKey("Gonderen")]
        public int GonderenId { get; set; }
        public Kullanici Gonderen { get; set; }


        public string Mesaj { get; set; }
        public DateTime tarih { get; set; }=DateTime.Now;
    }
}
