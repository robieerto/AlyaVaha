using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Material
    {
        public int Id { get; set; }

        [TsProperty(ForceNullable = true)]
        public string? NazovMaterialu { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumVytvorenia { get; set; }

        [TsProperty(ForceNullable = true)]
        public DateTime? DatumUpravy { get; set; }

        [TsProperty(ForceNullable = true)]
        public double? HmotnostMaterialu { get; set; }
    }
}
