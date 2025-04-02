using Reinforced.Typings.Attributes;

namespace AlyaVaha.Models
{
    [TsInterface]
    public class Nastavenia
    {
        public bool DatumACasV1Stlpci { get; set; } = false;
        // Sirena pri nepribudani
        public bool SirenaPriNepribudani { get; set; } = false;
        public double CasSirenyPriNepribudani { get; set; } = 60;
        public double VahaSirenyPriNepribudani { get; set; } = 10;
        // Sirena pri neodbudani
        public bool SirenaPriNeodbudani { get; set; } = false;
        public double CasSirenyPriNeodbudani { get; set; } = 60;
        public double VahaSirenyPriNeodbudani { get; set; } = 10;
    }
}
