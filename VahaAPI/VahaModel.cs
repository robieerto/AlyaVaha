using Reinforced.Typings.Attributes;
using System.ComponentModel;

namespace VahaAPI
{
    [TsInterface]
    public class VahaModel
    {
        [TsProperty(ForceNullable = true)]
        [DisplayName("NV")]
        public float? VahaNavazovania { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("BV")]
        public float? BruttoVaha { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("CV")]
        public float? CelkovaVaha { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("PC")]
        public int? PocetVyrobenychCyklovVazenia { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SP")]
        public float? PoslednaDokoncenaCelkovaVaha { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("DP")]
        public int? PoslednyDokoncenyPocetDavok { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("VD")]
        public float? PozadovanaVahaDavky { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("CD")]
        public int? CasCykluDavky { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("PV")]
        public float? PozadovanaCelkovaVaha { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("PD")]
        public int? PozadovanyPocetDavok { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SD")]
        public int? PocetDavokSirena { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SH")]
        public float? VahaSirena { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SN")]
        public int? StavNavazovania { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("EV")]
        public int? ErrorStavVahy { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("PR")]
        public int? ErrorStavPrevodnikaVahy { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("VC")]
        public float? VykonCelkovy { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("VA")]
        public float? VykonAktualny { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SS")]
        public int? StavSireny { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SV")]
        public int? StavVibratora { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("TM")]
        public string? AktualnyCas { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("DT")]
        public string? AktualnyDatum { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("NC")]
        public string? DatumCasStartuNavazovania { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("CK")]
        public int? CasDoKoncaDavky { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("LG")]
        public string? TabulkaUdalosti { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("KH")]
        public int? StavHornejKlapky { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("KD")]
        public int? StavDolnejKlapky { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("IO")]
        public string? DigitalneVstupy { get; set; }
        //public ProgramVahaInput? DigitalneVstupy { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("OO")]
        public string? DigitalneVystupy { get; set; }
        //public ProgramVahaOutput? DigitalneVystupy { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("VS")]
        public string? VerziaSoftware { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("SR")]
        public int? StavRiadeniaNavazovania { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("NN")]
        public string? TabulkaVazeni { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("MA")]
        public int? IdCisloMaterialu { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("OD")]
        public int? IdOdbernehoMiesta { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("KA")]
        public int? IdSmerovaciehoMiesta { get; set; }

        [TsProperty(ForceNullable = true)]
        [DisplayName("US")]
        public int? IdCisloPracovnika { get; set; }
    }

    [TsInterface]
    public class ProgramVahaInput
    {
        public bool HornaKlapkaOtvorena { get; set; }
        public bool HornaKlapkaZatvorena { get; set; }
        public bool DolnaKlapkaOtvorena { get; set; }
        public bool DolnaKlapkaZatvorena { get; set; }
        public bool RucnyRezim { get; set; }
        public bool Napajanie24V { get; set; }
        public bool BlokovanieVypustenia { get; set; }
        public bool HornaKlapkaStred { get; set; }
    }

    [TsInterface]
    public class ProgramVahaOutput
    {
        public bool HornuKlapkuOtvor { get; set; }
        public bool HornuKlapkuZatvor { get; set; }
        public bool DolnuKlapkuOtvor { get; set; }
        public bool DolnuKlapkuZatvor { get; set; }
        public bool Sirena { get; set; }
        public bool NavazovanieBezi { get; set; }
        public bool Porucha { get; set; }
        public bool Vibrator { get; set; }
        public bool Odfuk { get; set; }
        public bool Blokovanie1 { get; set; }
        public bool Blokovanie2 { get; set; }
        public bool HornuKlapkuNaStred { get; set; }
    }

    [TsEnum]
    public enum StavNavazovaniaPovel
    {
        StartNavazovania = 1,
        PrerusenieNavazovania = 2,
        PokracovanieNavazovania = 3,
        OkamziteUkoncenie = 4,
        UkonceniePoUkonceniDavky = 5,
    }

    [TsEnum]
    public enum StavNavazovania
    {
        Nedefinovane = 0,
        NavazovanieBezi = 1,
        NavazovaniePrerusene = 2,
        NavazovanieUkoncene = 3,
    }

    [TsEnum]
    public enum StavVahy
    {
        ZiadnaChyba = 0,
        VahaPodMinimum = 1,
        VahaNadMaximum = 2,
    }

    [TsEnum]
    public enum StavPrevodnika
    {
        ZiadnaChyba = 0,
        PrevodNemozeDokoncitVcas = 16,
        ElektronikaSaNemozeSpojit = 32,
        NemozeSaSpojitSTenzometrom = 48,
        ZarusenieEMCRusenim = 64,
    }

    [TsEnum]
    public enum StavSirenyPovel
    {
        VypniSirenu = 1,
        ZapniSirenu = 2,
        ZapniSirenuPrerusovane = 3,
    }

    [TsEnum]
    public enum StavSireny
    {
        SirenaVypnuta = 0,
        SirenaZapnuta = 1,
        SirenaZapnutaPrerusovane = 2,
    }

    [TsEnum]
    public enum StavVibratoraPovel
    {
        VypniVibrator = 1,
        ZapniVibrator = 2,
    }

    [TsEnum]
    public enum StavVibratora
    {
        VibratorVypnuty = 0,
        VibratorZapnuty = 1,
    }

    [TsEnum]
    public enum StavKlapkyPovel
    {
        OtvorKlapku = 1,
        ZatvorKlapku = 2,
    }

    [TsEnum]
    public enum StavKlapky
    {
        Nedefinovane = 0,
        KlapkaOtvorena = 1,
        KlapkaSaOtvara = 2,
        KlapkaZatvorena = 3,
        KlapkaSaZatvara = 4,
        KlapkaVPoruche = 5,
    }

    [TsEnum]
    public enum StavRiadeniaNavazovania
    {
        Nedefinovane = 0,
        CakaNaStart = 1,
        CakaPredNulovanimNaZatvorenieKlapiek = 2,
        CakaNaVynulovanieVahy = 3,
        NavazujeSa = 4,
        CakaPredOdvazenimNaZatvorenieKlapiek = 5,
        CakaNaOdvazenie = 6,
        BlokovanieVypustenia = 7,
        PrebiehaVyprazdnovanie = 8,
        CakaPoVyprazdneni = 9,
        CakaPoVyprazdneniNaZatvorenieKlapiek = 10,
        CakaPredNovymCyklom = 11,
        CakaVPreruseni = 12,
        JeVPoruche = 13,
    }

    [TsEnum]
    public enum TypNavazovania
    {
        Donekonecna = 0,
        PozadovanaVaha = 1,
        PozadovanyPocetDavok = 2,
    }
}
