using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Models
{
    public class DanisanEgzersizProgrami
    {
        [Key]
        public int Id { get; set; }
        public string EgzersizAdi { get; set; }
        public string Hedefleri { get; set; }
        public int SetSayisi { get; set; }
        public int TekrarSayisi { get; set; }
        public string VideoRehberi { get; set; }
        public string PrograminSuresi { get; set; }
        public DateTime ProgramaBaslamaTarihi { get; set; }=DateTime.Now;

        [ForeignKey("Kullanici")]
        public int DanisanId { get; set; }
        public Kullanici Kullanici { get; set; }


        [ForeignKey("Antrenor")]
        public int AntrenorId { get; set; }
        public Kullanici Antrenor { get; set; }
    }
}
