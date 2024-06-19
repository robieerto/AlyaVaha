import store from '@/store'

import notify from 'devextreme/ui/notify'

function initMessageHandler() {
  ;(window.external as any).receiveMessage((responseStr: string) => {
    const response = JSON.parse(responseStr) as AlyaVaha.IWindowCommand

    switch (response.Command) {
      case 'ActualData':
        store.actualData = JSON.parse(response.Value) as VahaAPI.IVahaModel
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
      case 'GetMaterialy': {
        store.materialy = JSON.parse(response.Value) as AlyaVaha.Models.IMaterial[]
        store.materialyLoading = false
        break
      }
      default:
        break
    }
  })
}

function sendCommand(command: string, value: any = {}) {
  const message = { Command: command } as AlyaVaha.IWindowCommand
  message.Value = JSON.stringify(value)
  ;(window.external as any).sendMessage(JSON.stringify(message))
}

export { initMessageHandler, sendCommand }
