import * as types from '@/types'
import store from '@/store'

import notify from 'devextreme/ui/notify'

function initMessageHandler() {
  ;(window.external as any).receiveMessage((responseStr: string) => {
    const response = JSON.parse(responseStr) as types.WindowCommand

    switch (response.Command) {
      case 'ActualData':
        store.actualData = JSON.parse(response.Value) as types.VahaModel
        break
      case 'SetValues': {
        const notSetValues = JSON.parse(response.Value) as string[]
        if (notSetValues.length > 0) {
          notify({
            message: `Nepodarilo sa nastaviť:
             ${notSetValues.join(', ')}`,
            type: 'error',
            displayTime: 5000,
            position: 'top center'
          })
        }
        break
      }
      default:
        break
    }
  })
}

export { initMessageHandler }
