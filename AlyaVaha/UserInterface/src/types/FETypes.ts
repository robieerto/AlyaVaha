module FE {
  export interface IVahaModel {
    VahaNavazovania: number
    BruttoVaha: number
    CelkovaVaha: number
    PocetVyrobenychCyklovVazenia: number
    PoslednaDokoncenaCelkovaVaha: number
    PoslednyDokoncenyPocetDavok: number
    PozadovanaVahaDavky: number
    CasCykluDavky: number
    PozadovanaCelkovaVaha: number
    PozadovanyPocetDavok: number
    PocetDavokSirena: number
    VahaSirena: number
    StavNavazovania: number
    ErrorStavVahy: number
    ErrorStavPrevodnikaVahy: number
    VykonCelkovy: number
    VykonAktualny: number
    StavSireny: number
    StavVibratora: number
    AktualnyCas: string
    AktualnyDatum: string
    DatumCasStartuNavazovania: string
    CasDoKoncaDavky: number
    TabulkaUdalosti: string
    StavHornejKlapky: number
    StavDolnejKlapky: number
    DigitalneVstupy: string
    DigitalneVystupy: string
    VerziaSoftware: string
    StavRiadeniaNavazovania: number
    TabulkaVazeni: string
    IdCisloMaterialu: number
    IdOdbernehoMiesta: number
    IdSmerovaciehoMiesta: number
    IdCisloPracovnika: number
  }
}
