import { reactive } from 'vue'

const store = reactive({
  actualData: {} as VahaAPI.IVahaModel,
  navazovania: [] as AlyaVaha.Models.INavazovanie[],
  zariadenia: [] as AlyaVaha.Models.IZariadenie[],
  materialy: [] as AlyaVaha.Models.IMaterial[],
  zasobniky: [] as AlyaVaha.Models.IZasobnik[],
  navazovaniaLoading: true,
  materialyLoading: true,
  zasobnikyLoading: true
})

export default store
