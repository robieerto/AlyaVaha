using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Navazovanie
    {
        public int Id { get; set; }
        public DateTime? DatumStartu { get; set; }
        public string? CasStartu { get; set; }
        public DateTime? DatumKonca { get; set; }
        public string? CasKonca { get; set; }
        public int? ZariadenieId { get; set; }
        public Zariadenie? Zariadenie { get; set; }
        public double? NavazeneMnozstvo { get; set; }
        public int? NavazenyPocetDavok { get; set; }
        public double? PozadovaneMnozstvo { get; set; }
        public int? PozadovanyPocetDavok { get; set; }
        public double? VelkostDavky { get; set; }
        public int? OdkialId { get; set; }
        public Zasobnik? Odkial { get; set; }
        public int? KamId { get; set; }
        public Zasobnik? Kam { get; set; }
        public int? MaterialId { get; set; }
        public Material? Material { get; set; }
        public int? UzivatelId { get; set; }
        public Uzivatel? Uzivatel { get; set; }
    }
}
