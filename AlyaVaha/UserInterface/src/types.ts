export interface VahaModel {
  vahaNavazovania: number
  bruttoVaha: number
  celkovaVaha: number
  pocetVyrobenychCyklovVazenia: number
  poslednaDokoncenaCelkovaVaha: number
  poslednyDokoncenyPocetDavok: number
  pozadovanaVahaDavky: number
  casCykluDavky: number
  pozadovanaCelkovaVaha: number
  pozadovanyPocetDavok: number
  pocetDavokSirena: number
  vahaSirena: number
  stavNavazovania: number
  errorStavVahy: number
  errorStavPrevodnikaVahy: number
  vykonCelkovy: number
  vykonAktualny: number
  stavSireny: number
  stavVibratora: number
  aktualnyCas: string | null
  aktualnyDatum: string | null
  datumCasStartuNavazovania: string | null
  casDoKoncaDavky: number
  tabulkaUdalosti: string | null
  stavHornejKlapky: number
  stavDolnejKlapky: number
  digitalneVstupy: string | null
  digitalneVystupy: string | null
  verziaSoftware: string | null
  stavRiadeniaNavazovania: number
  tabulkaVazeni: string | null
  idCisloMaterialu: number
  idOdbernehoMiesta: number
  idSmerovaciehoMiesta: number
  idCisloPracovnika: number
}

export interface VahaInput {
  hornaKlapkaOtvorena: boolean
  hornaKlapkaZatvorena: boolean
  dolnaKlapkaOtvorena: boolean
  dolnaKlapkaZatvorena: boolean
  rucnyRezim: boolean
  napajanie24V: boolean
  blokovanieVypustenia: boolean
  hornaKlapkaStred: boolean
}

export interface VahaOutput {
  hornuKlapkuOtvor: boolean
  hornuKlapkuZatvor: boolean
  dolnuKlapkuOtvor: boolean
  dolnuKlapkuZatvor: boolean
  sirena: boolean
  navazovanieBezi: boolean
  porucha: boolean
  vibrator: boolean
  odfuk: boolean
  blokovanie1: boolean
  blokovanie2: boolean
  hornuKlapkuNaStred: boolean
}
