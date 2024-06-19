import { reactive } from 'vue'

const store = reactive({
  actualData: {} as VahaAPI.IVahaModel,
  materialy: [] as AlyaVaha.Models.IMaterial[],
  materialyLoading: true
})

export default store
