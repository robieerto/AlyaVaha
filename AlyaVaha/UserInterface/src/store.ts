import { reactive } from 'vue'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from '@/messages'

const initDataUzivatelZariadenie = {
  isUserLoggedIn: null as boolean | null,
  isUserAdmin: null as boolean | null,
  connected: false,
  user: null as AlyaVaha.Models.IUzivatel | null,
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
  actualStateTexts: {} as Partial<Record<keyof typeof Messages, string>>
}

const store = reactive({
  ...initDataUzivatelZariadenie,
  zariadenie: null as AlyaVaha.Models.IZariadenie | null,
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
  statistikyLoading: false,
  isStartNavazovanieModalOpened: false,
  isNavazovanieInitSuccess: false,
  isDateTimePrehlad: false,
  isDateTimeStatistiky: false,
  isNavazovaniaDeleting: false
})

export const resetLoggedInZariadenieData = () => {
  store.isUserLoggedIn = initDataUzivatelZariadenie.isUserLoggedIn
  store.isUserAdmin = initDataUzivatelZariadenie.isUserAdmin
  store.user = initDataUzivatelZariadenie.user
  store.connected = initDataUzivatelZariadenie.connected
  store.actualData = initDataUzivatelZariadenie.actualData
  store.actualInputs = initDataUzivatelZariadenie.actualInputs
  store.actualOutputs = initDataUzivatelZariadenie.actualOutputs
  store.actualStateTexts = initDataUzivatelZariadenie.actualStateTexts
}

export default store
