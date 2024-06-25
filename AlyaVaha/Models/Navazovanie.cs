using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Navazovanie
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? CasStartu { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? CasKonca { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? ZariadenieId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Zariadenie? Zariadenie { get; set; }

        [TsProperty(ForceNullable = true)]
        public double? NavazeneMnozstvo { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? NavazenyPocetDavok { get; set; }

        [TsProperty(ForceNullable = true)]
        public double? PozadovaneMnozstvo { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? PozadovanyPocetDavok { get; set; }

        [TsProperty(ForceNullable = true)]
        public double? VelkostDavky { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? OdkialId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Zasobnik? Odkial { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? KamId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Zasobnik? Kam { get; set; }

        [TsProperty(ForceNullable = true)]
        public int? UzivatelId { get; set; }

        [TsProperty(ForceNullable = true)]
        public Uzivatel? Uzivatel { get; set; }
    }
}
