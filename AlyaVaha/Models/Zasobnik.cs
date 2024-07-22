using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Zasobnik
    {
        public int Id { get; set; }
        public string? NazovZasobnika { get; set; }
        public DateTime? DatumVytvorenia { get; set; }
        public DateTime? DatumUpravy { get; set; }
        public string? Skratka { get; set; }
        public bool? CestaTam { get; set; }
        public bool? CestaSpat { get; set; }
    }
}
