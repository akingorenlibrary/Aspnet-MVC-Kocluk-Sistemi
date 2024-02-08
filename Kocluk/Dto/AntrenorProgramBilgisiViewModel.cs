using Kocluk.Models;

namespace Kocluk.Dto
{
    public class AntrenorProgramBilgisiViewModel
    {
        public DanisanBeslenmeProgrami danisanBeslenmeProgrami { get; set; }
        public DanisanEgzersizProgrami danisanEgzersizProgrami { get; set; }
        public Kullanici antrenor { get; set; }
    }
}
