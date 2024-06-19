using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Cesta
    {
        public int Id { get; set; }
        public DateTime? DatumVytvorenia { get; set; }
        public DateTime? DatumUpravy { get; set; }
        public int? ZariadenieId { get; set; }
        public Zariadenie? Zariadenie { get; set; }
        public int? ZasobnikId { get; set; }
        public Zasobnik? Zasobnik { get; set; }
        public bool? DoVahy { get; set; }
        public bool? ZVahy { get; set; }
    }
}
