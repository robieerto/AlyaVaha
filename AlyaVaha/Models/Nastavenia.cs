using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Nastavenia
    {
        public bool SirenaPriNepribudani { get; set; } = false;
        public bool SirenaPriNeodbudani { get; set; } = false;
        public double CasSirenyPriNepribudani { get; set; } = 0;
        public double CasSirenyPriNeodbudani { get; set; } = 0;
    }
}
