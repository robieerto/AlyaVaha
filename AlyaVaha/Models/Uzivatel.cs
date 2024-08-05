using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Uzivatel
    {
        public int Id { get; set; }
        public DateTime? DatumVytvorenia { get; set; }
        public DateTime? DatumUpravy { get; set; }
        public string? Login { get; set; }
        public string? Heslo { get; set; }
        public string? Email { get; set; }
        public bool? JeAdmin { get; set; }
        public bool? JeAktivny { get; set; }
    }
}
