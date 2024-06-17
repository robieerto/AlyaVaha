using System.ComponentModel;

namespace VahaAPI
{
    public class VahaModel
    {
        [DisplayName("NV")]
        public float? VahaNavazovania { get; set; }

        [DisplayName("BV")]
        public float? BruttoVaha { get; set; }

        [DisplayName("CV")]
        public float? CelkovaVaha { get; set; }

        [DisplayName("PC")]
        public int? PocetVyrobenychCyklovVazenia { get; set; }

        [DisplayName("SP")]
        public float? PoslednaDokoncenaCelkovaVaha { get; set; }

        [DisplayName("DP")]
        public int? PoslednyDokoncenyPocetDavok { get; set; }

        [DisplayName("VD")]
        public float? PozadovanaVahaDavky { get; set; }

        [DisplayName("CD")]
        public int? CasCykluDavky { get; set; }

        [DisplayName("PV")]
        public float? PozadovanaCelkovaVaha { get; set; }

        [DisplayName("PD")]
        public int? PozadovanyPocetDavok { get; set; }

        [DisplayName("SD")]
        public int? PocetDavokSirena { get; set; }

        [DisplayName("SH")]
        public float? VahaSirena { get; set; }

        [DisplayName("SN")]
        public int? StavNavazovania { get; set; }

        [DisplayName("EV")]
        public int? ErrorStavVahy { get; set; }

        [DisplayName("PR")]
        public int? ErrorStavPrevodnikaVahy { get; set; }

        [DisplayName("VC")]
        public float? VykonCelkovy { get; set; }

        [DisplayName("VA")]
        public float? VykonAktualny { get; set; }

        [DisplayName("SS")]
        public int? StavSireny { get; set; }

        [DisplayName("SV")]
        public int? StavVibratora { get; set; }

        [DisplayName("TM")]
        public string? AktualnyCas { get; set; }

        [DisplayName("DT")]
        public string? AktualnyDatum { get; set; }

        [DisplayName("NC")]
        public string? DatumCasStartuNavazovania { get; set; }

        [DisplayName("CK")]
        public int? CasDoKoncaDavky { get; set; }

        [DisplayName("LG")]
        public string? TabulkaUdalosti { get; set; }

        [DisplayName("KH")]
        public int? StavHornejKlapky { get; set; }

        [DisplayName("KD")]
        public int? StavDolnejKlapky { get; set; }

        [DisplayName("IO")]
        public string? DigitalneVstupy { get; set; }
        //public ProgramVahaInput? DigitalneVstupy { get; set; }

        [DisplayName("OO")]
        public string? DigitalneVystupy { get; set; }
        //public ProgramVahaOutput? DigitalneVystupy { get; set; }

        [DisplayName("VS")]
        public string? VerziaSoftware { get; set; }

        [DisplayName("SR")]
        public int? StavRiadeniaNavazovania { get; set; }

        [DisplayName("NN")]
        public string? TabulkaVazeni { get; set; }

        [DisplayName("MA")]
        public int? IdCisloMaterialu { get; set; }

        [DisplayName("OD")]
        public int? IdOdbernehoMiesta { get; set; }

        [DisplayName("KA")]
        public int? IdSmerovaciehoMiesta { get; set; }

        [DisplayName("US")]
        public int? IdCisloPracovnika { get; set; }
    }

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
}
