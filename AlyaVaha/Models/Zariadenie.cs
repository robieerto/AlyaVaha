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
    }
}
