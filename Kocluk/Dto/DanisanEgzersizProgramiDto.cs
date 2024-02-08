using Kocluk.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Dto
{
    public class DanisanEgzersizProgramiDto
    {
        public string EgzersizAdi { get; set; }
        public string Hedefleri { get; set; }
        public int SetSayisi { get; set; }
        public int TekrarSayisi { get; set; }
        public string VideoRehberi { get; set; }
        public string PrograminSuresi { get; set; }
    }
}
