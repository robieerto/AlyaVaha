import { reactive } from 'vue'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from '@/messages'

const store = reactive({
  isUserLoggedIn: null as boolean | null,
  isUserAdmin: null as boolean | null,
  user: null as any,
  connected: false,
  wasConnected: false,
  actualData: {} as VahaAPI.IVahaModel,
  actualInputs: {
    HornaKlapkaOtvorena: false,
    HornaKlapkaZatvorena: false,
    DolnaKlapkaOtvorena: false,
    DolnaKlapkaZatvorena: false,
    RucnyRezim: false,
    Napajanie24V: false,
    BlokovanieVypustenia: false,
    StopNavazovania: false,
    HornaKlapkaStred: false
  } as VahaAPI.IProgramVahaInput,
  actualOutputs: {
    HornuKlapkuOtvor: false,
    HornuKlapkuZatvor: false,
    DolnuKlapkuOtvor: false,
    DolnuKlapkuZatvor: false,
    Sirena: false,
    NavazovanieBezi: false,
    Porucha: false,
    Vibrator: false,
    Odfuk: false,
    Blokovanie1: false
  } as VahaAPI.IProgramVahaOutput,
  actualStateTexts: {} as Partial<Record<keyof typeof Messages, string>>,
  navazovania: [] as AlyaVaha.Models.INavazovanie[],
  navazovaniaData: null,
  navazovaniaDataExport: null,
  zariadenia: [] as AlyaVaha.Models.IZariadenie[],
  uzivatelia: [] as AlyaVaha.Models.IUzivatel[],
  aktivniUzivatelia: [] as AlyaVaha.Models.IUzivatel[],
  materialy: [] as AlyaVaha.Models.IMaterial[],
  aktivneMaterialy: [] as AlyaVaha.Models.IMaterial[],
  zasobniky: [] as AlyaVaha.Models.IZasobnik[],
  zasobnikyDoVahy: [] as AlyaVaha.Models.IZasobnik[],
  zasobnikyZVahy: [] as AlyaVaha.Models.IZasobnik[],
  statistiky: [] as Number[],
  navazovaniaLoading: false,
  uzivateliaLoading: true,
  materialyLoading: true,
  zasobnikyLoading: true,
  statistikyLoading: true,
  isStartNavazovanieModalOpened: false,
  isNavazovanieInitSuccess: false,
  isDateTime: false
})

export default store
