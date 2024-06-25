import store from '@/store'
import { notify, shortNotify } from '@/utils/helpers'

function initCommandHandler() {
  try {
    ;(window.external as any).receiveMessage(async (responseStr: string) => {
      return new Promise(() => {
        try {
          const response = JSON.parse(responseStr) as AlyaVaha.IWindowCommand

          switch (response.Command) {
            case 'ActualData':
              store.actualData = JSON.parse(response.Value) as VahaAPI.IVahaModel
              break
            case 'SetValues': {
              const notSetValues = JSON.parse(response.Value) as string[]
              if (notSetValues.length > 0) {
                notify(`Nepodarilo sa nastaviť: ${notSetValues.join(', ')}`, 'error')
              } else {
                notify('Hodnoty boli úspešne nastavené', 'success')
              }
              break
            }
            case 'GetZariadenia': {
              store.zariadenia = JSON.parse(response.Value) as AlyaVaha.Models.IZariadenie[]
              break
            }
            case 'GetMaterialy': {
              store.materialy = JSON.parse(response.Value) as AlyaVaha.Models.IMaterial[]
              store.materialyLoading = false
              break
            }
            case 'GetZasobniky': {
              store.zasobniky = JSON.parse(response.Value) as AlyaVaha.Models.IZasobnik[]
              store.zasobnikyLoading = false
              break
            }
            case 'GetNavazovania': {
              store.navazovania = JSON.parse(response.Value) as AlyaVaha.Models.INavazovanie[]
              store.navazovaniaLoading = false
              break
            }
            default: {
              if (
                response.Command.startsWith('Add') ||
                response.Command.startsWith('Update') ||
                response.Command.startsWith('Delete')
              ) {
                shortNotify(JSON.parse(response.Value), 'info')
              }
              if (response.Command == 'AddMaterial' || response.Command == 'UpdateMaterial') {
                sendCommand('GetMaterialy')
              }
              if (response.Command == 'AddZasobnik' || response.Command == 'UpdateZasobnik') {
                sendCommand('GetZasobniky')
              }
              break
            }
          }
        } catch (e) {
          /* empty */
        }
      })
    })
  } catch (e) {
    /* empty */
  }
}

async function sendCommand(command: string, value: any = {}) {
  const message = { Command: command } as AlyaVaha.IWindowCommand
  message.Value = JSON.stringify(value)
  try {
    ;(window.external as any).sendMessage(JSON.stringify(message))
  } catch (e) {
    /* empty */
  }
}

async function getAllData() {
  sendCommand('GetZariadenia')
  sendCommand('GetMaterialy')
  sendCommand('GetZasobniky')
  sendCommand('GetNavazovania')
}

export { initCommandHandler, sendCommand, getAllData }
