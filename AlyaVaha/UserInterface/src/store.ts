import { reactive } from 'vue'
import * as types from '@/types'

const store = reactive({
  actualData: {} as types.VahaModel
})

export default store
