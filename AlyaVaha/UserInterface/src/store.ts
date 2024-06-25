import { reactive } from 'vue'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from '@/messages'

const store = reactive({
  actualData: {} as VahaAPI.IVahaModel,
  actualInputs: {} as VahaAPI.IProgramVahaInput,
  actualStateTexts: {} as Partial<Record<keyof typeof Messages, string>>,
  navazovania: [] as AlyaVaha.Models.INavazovanie[],
  zariadenia: [] as AlyaVaha.Models.IZariadenie[],
  materialy: [] as AlyaVaha.Models.IMaterial[],
  zasobniky: [] as AlyaVaha.Models.IZasobnik[],
  navazovaniaLoading: true,
  materialyLoading: true,
  zasobnikyLoading: true,
  isStartNavazovanieModalOpened: false
})

export default store
