using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Material
    {
        public int Id { get; set; }
        public string? NazovMaterialu { get; set; }
        public DateTime? DatumVytvorenia { get; set; }
        public DateTime? DatumUpravy { get; set; }
        public double? HmotnostMaterialu { get; set; }
    }
}
