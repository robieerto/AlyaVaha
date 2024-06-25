using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Cesta
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumVytvorenia { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumUpravy { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? ZariadenieId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Zariadenie? Zariadenie { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? ZasobnikId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Zasobnik? Zasobnik { get; set; }

        [TsProperty(ForceNullable = true)]
        public bool? DoVahy { get; set; }

        [TsProperty(ForceNullable = true)]
        public bool? ZVahy { get; set; }
    }
}
