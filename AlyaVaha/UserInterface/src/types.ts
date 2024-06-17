export interface WindowCommand {
  Command: string
  Value: string
}

export interface VahaModel {
  VahaNavazovania: number | null
  BruttoVaha: number | null
  CelkovaVaha: number | null
  PocetVyrobenychCyklovVazenia: number | null
  PoslednaDokoncenaCelkovaVaha: number | null
  PoslednyDokoncenyPocetDavok: number | null
  PozadovanaVahaDavky: number | null
  CasCykluDavky: number | null
  PozadovanaCelkovaVaha: number | null
  PozadovanyPocetDavok: number | null
  PocetDavokSirena: number | null
  VahaSirena: number | null
  StavNavazovania: number | null
  ErrorStavVahy: number | null
  ErrorStavPrevodnikaVahy: number | null
  VykonCelkovy: number | null
  VykonAktualny: number | null
  StavSireny: number | null
  StavVibratora: number | null
  AktualnyCas: string | null
  AktualnyDatum: string | null
  DatumCasStartuNavazovania: string | null
  CasDoKoncaDavky: number | null
  TabulkaUdalosti: string | null
  StavHornejKlapky: number | null
  StavDolnejKlapky: number | null
  DigitalneVstupy: string | null
  DigitalneVystupy: string | null
  VerziaSoftware: string | null
  StavRiadeniaNavazovania: number | null
  TabulkaVazeni: string | null
  IdCisloMaterialu: number | null
  IdOdbernehoMiesta: number | null
  IdSmerovaciehoMiesta: number | null
  IdCisloPracovnika: number | null
}

export interface ProgramVahaInput {
  HornaKlapkaOtvorena: boolean
  HornaKlapkaZatvorena: boolean
  DolnaKlapkaOtvorena: boolean
  DolnaKlapkaZatvorena: boolean
  RucnyRezim: boolean
  Napajanie24V: boolean
  BlokovanieVypustenia: boolean
  HornaKlapkaStred: boolean
}

export interface ProgramVahaOutput {
  HornuKlapkuOtvor: boolean
  HornuKlapkuZatvor: boolean
  DolnuKlapkuOtvor: boolean
  DolnuKlapkuZatvor: boolean
  Sirena: boolean
  NavazovanieBezi: boolean
  Porucha: boolean
  Vibrator: boolean
  Odfuk: boolean
  Blokovanie1: boolean
  Blokovanie2: boolean
  HornuKlapkuNaStred: boolean
}

export enum StavNavazovaniaPovel {
  StartNavazovania = 1,
  PrerusenieNavazovania = 2,
  PokracovanieNavazovania = 3,
  OkamziteUkoncenie = 4,
  UkonceniePoUkonceniDavky = 5
}

export enum StavNavazovania {
  Nedefinovane = 0,
  NavazovanieBezi = 1,
  NavazovaniePrerusene = 2,
  NavazovanieUkoncene = 3
}

export enum StavVahy {
  ZiadnaChyba = 0,
  VahaPodMinimum = 1,
  VahaNadMaximum = 2
}

export enum StavPrevodnika {
  ZiadnaChyba = 0,
  PrevodNemozeDokoncitVcas = 16,
  ElektronikaSaNemozeSpojit = 32,
  NemozeSaSpojitSTenzometrom = 48,
  ZarusenieEMCRusenim = 64
}

export enum StavSirenyPovel {
  VypniSirenu = 1,
  ZapniSirenu = 2,
  ZapniSirenuPrerusovane = 3
}

export enum StavSireny {
  SirenaVypnuta = 0,
  SirenaZapnuta = 1,
  SirenaZapnutaPrerusovane = 2
}

export enum StavVibratoraPovel {
  VypniVibrator = 1,
  ZapniVibrator = 2
}

export enum StavVibratora {
  VibratorVypnuty = 0,
  VibratorZapnuty = 1
}

export enum StavKlapkyPovel {
  OtvorKlapku = 1,
  ZatvorKlapku = 2
}

export enum StavKlapky {
  Nedefinovane = 0,
  KlapkaOtvorena = 1,
  KlapkaSaOtvara = 2,
  KlapkaZatvorena = 3,
  KlapkaSaZatvara = 4,
  KlapkaVPoruche = 5
}

export enum StavRiadeniaNavazovania {
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
  JeVPoruche = 13
}
