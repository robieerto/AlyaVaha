using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Uzivatel
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? Login { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? Heslo { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? Email { get; set; }

        [TsProperty(ForceNullable = true)]
        public bool? JeAdmin { get; set; }
    }
}
