using Kocluk.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kocluk.Dto
{
    public class DanisanBeslenmeProgramiDto
    {
        public string Hedef { get; set; }
        public string GunlukOgunler { get; set; }
        public string KaloriHedefi { get; set; }
    }
}
