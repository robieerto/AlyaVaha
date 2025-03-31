using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Zariadenie
    {
        public int Id { get; set; }
        public string? NazovZariadenia { get; set; }
        public string? IpAdresa { get; set; }
        public int? Port { get; set; }
        public int PocetNavazeni { get; set; }
        public double NavazeneMnozstvo { get; set; }
        public int NavazenyPocetDavok { get; set; }
        public string? Nastavenia { get; set; }
    }
}
