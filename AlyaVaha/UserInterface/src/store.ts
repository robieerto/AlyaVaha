import { reactive } from 'vue'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from '@/messages'

const store = reactive({
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
  materialy: [] as AlyaVaha.Models.IMaterial[],
  zasobniky: [] as AlyaVaha.Models.IZasobnik[],
  navazovaniaLoading: true,
  materialyLoading: true,
  zasobnikyLoading: true,
  isStartNavazovanieModalOpened: false,
  isNavazovanieInitSuccess: false
})

export default store
