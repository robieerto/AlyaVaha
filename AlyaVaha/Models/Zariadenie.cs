using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Zariadenie
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? NazovZariadenia { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? IpAdresa { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? Port { get; set; }
    }
}
