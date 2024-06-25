using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Zasobnik
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? NazovZasobnika { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumVytvorenia { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumUpravy { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? Skratka { get; set; }
    }
}
