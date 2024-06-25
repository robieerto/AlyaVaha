import { notify, shortNotify } from '@/utils/helpers'

import store from '@/store'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from './messages'

function initCommandHandler() {
  try {
    ;(window.external as any).receiveMessage(async (responseStr: string) => {
      return new Promise(() => {
        try {
          const response = JSON.parse(responseStr) as AlyaVaha.IWindowCommand
          switch (response.Command) {
            case 'ActualData':
              store.actualData = JSON.parse(response.Value!) as VahaAPI.IVahaModel
              setActualInputs()
              setActualStateTexts()
              break
            case 'SetValues': {
              const notSetValues = JSON.parse(response.Value!) as string[]
              if (notSetValues.length > 0) {
                notify(`Nepodarilo sa nastaviť: ${notSetValues.join(', ')}`, 'error')
              } else {
                notify('Hodnoty boli úspešne nastavené', 'success')
              }
              break
            }
            case 'GetZariadenia': {
              store.zariadenia = JSON.parse(response.Value!) as AlyaVaha.Models.IZariadenie[]
              break
            }
            case 'GetMaterialy': {
              store.materialy = JSON.parse(response.Value!) as AlyaVaha.Models.IMaterial[]
              store.materialyLoading = false
              break
            }
            case 'GetZasobniky': {
              store.zasobniky = JSON.parse(response.Value!) as AlyaVaha.Models.IZasobnik[]
              store.zasobnikyLoading = false
              break
            }
            case 'GetNavazovania': {
              store.navazovania = JSON.parse(response.Value!) as AlyaVaha.Models.INavazovanie[]
              store.navazovaniaLoading = false
              break
            }
            default: {
              if (
                response.Command.startsWith('Add') ||
                response.Command.startsWith('Update') ||
                response.Command.startsWith('Delete')
              ) {
                shortNotify(JSON.parse(response.Value!), 'info')
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

async function setActualInputs() {
  const vstupy = store.actualData.DigitalneVstupy
  if (vstupy) {
    for (let i = 0; i < vstupy.length; i++) {
      switch (i) {
        case 0:
          store.actualInputs.HornaKlapkaOtvorena = vstupy[i] == '1'
          break
        case 1:
          store.actualInputs.HornaKlapkaZatvorena = vstupy[i] == '1'
          break
        case 2:
          store.actualInputs.DolnaKlapkaOtvorena = vstupy[i] == '1'
          break
        case 3:
          store.actualInputs.DolnaKlapkaZatvorena = vstupy[i] == '1'
          break
        case 4:
          store.actualInputs.RucnyRezim = vstupy[i] == '1'
          break
        case 5:
          store.actualInputs.Napajanie24V = vstupy[i] == '1'
          break
        case 6:
          store.actualInputs.BlokovanieVypustenia = vstupy[i] == '1'
          break
        case 7:
          store.actualInputs.StopNavazovania = vstupy[i] == '1'
          break
        case 11:
          store.actualInputs.HornaKlapkaStred = vstupy[i] == '1'
          break
      }
    }
  }
}

async function setActualStateTexts() {
  store.actualStateTexts = {
    StavNavazovania: getStavNavazovania(),
    StavVahy: getStavVahy(),
    StavPrevodnika: getStavPrevodnika(),
    StavSireny: getStavSireny(),
    StavVibratora: getStavVibratora(),
    StavHornejKlapky: getStavHornejKlapky(),
    StavDolnejKlapky: getStavDolnejKlapky(),
    StavRiadeniaNavazovania: getStavRiadeniaNavazovania()
  }
}

const getStavNavazovania = () =>
  Messages['StavNavazovania'][
    VahaAPI.StavNavazovania[
      store.actualData['StavNavazovania']!
    ] as keyof typeof Messages.StavNavazovania
  ]

const getStavVahy = () =>
  Messages['StavVahy'][
    VahaAPI.StavVahy[store.actualData['ErrorStavVahy']!] as keyof typeof Messages.StavVahy
  ]

const getStavPrevodnika = () =>
  Messages['StavPrevodnika'][
    VahaAPI.StavPrevodnika[
      store.actualData['ErrorStavPrevodnikaVahy']!
    ] as keyof typeof Messages.StavPrevodnika
  ]

const getStavSireny = () =>
  Messages['StavSireny'][
    VahaAPI.StavSireny[store.actualData['StavSireny']!] as keyof typeof Messages.StavSireny
  ]

const getStavVibratora = () =>
  Messages['StavVibratora'][
    VahaAPI.StavVibratora[store.actualData['StavVibratora']!] as keyof typeof Messages.StavVibratora
  ]

const getStavHornejKlapky = () =>
  Messages['StavHornejKlapky'][
    VahaAPI.StavKlapky[
      store.actualData['StavHornejKlapky']!
    ] as keyof typeof Messages.StavHornejKlapky
  ]

const getStavDolnejKlapky = () =>
  Messages['StavDolnejKlapky'][
    VahaAPI.StavKlapky[
      store.actualData['StavDolnejKlapky']!
    ] as keyof typeof Messages.StavDolnejKlapky
  ]

const getStavRiadeniaNavazovania = () =>
  Messages['StavRiadeniaNavazovania'][
    VahaAPI.StavRiadeniaNavazovania[
      store.actualData['StavRiadeniaNavazovania']!
    ] as keyof typeof Messages.StavRiadeniaNavazovania
  ]

export { initCommandHandler, sendCommand, getAllData }
