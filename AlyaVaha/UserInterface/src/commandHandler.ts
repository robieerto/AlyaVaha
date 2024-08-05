import { notify } from '@/utils/helpers'
import { useRoute } from 'vue-router'

import store from '@/store'
import router from '@/router'
import * as VahaAPI from '@/types/vahaTypes'
import { Messages } from './messages'

let timeoutId = setTimeout(() => {}, 0)

const afterTimeout = () => {
  if (!store.navazovaniaLoading && !store.statistikyLoading) {
    store.connected = false
  } else {
    timeoutId = setTimeout(afterTimeout, 3000)
  }
}

function initCommandHandler() {
  try {
    ;(window.external as any).receiveMessage(async (responseStr: string) => {
      return new Promise(() => {
        try {
          const response = JSON.parse(responseStr) as AlyaVaha.IWindowCommand
          switch (response.Command) {
            case 'Login': {
              const operationResult = JSON.parse(response.Value!) as AlyaVaha.IOperationResult
              if (operationResult.Success) {
                store.user = JSON.parse(operationResult.Data!) as AlyaVaha.Models.IUzivatel
                store.isUserAdmin = store.user.JeAdmin
                store.isUserLoggedIn = true
                getAllData()
              } else {
                store.isUserLoggedIn = false
                notify(operationResult.Message, 'error')
              }
              break
            }
            case 'GetLoggedInUser': {
              if (response.Value !== '"null"') {
                store.user = JSON.parse(response.Value!) as AlyaVaha.Models.IUzivatel
                store.isUserAdmin = store.user.JeAdmin
                store.isUserLoggedIn = true
                getAllData()
                const params = new URL(document.location.toString()).searchParams
                const redirectUrl = params.get('redirect')
                const path = window.location.href.substring(
                  window.location.href.lastIndexOf('/') + 1
                )
                router.push(redirectUrl ?? path)
              } else {
                store.isUserLoggedIn = false
                router.push('/')
              }
              break
            }
            case 'ActualData': {
              if (!store.isUserLoggedIn) {
                return
              }
              store.connected = true
              store.wasConnected = true
              clearTimeout(timeoutId)
              timeoutId = setTimeout(afterTimeout, 3000)

              store.actualData = JSON.parse(response.Value!) as VahaAPI.IVahaModel
              setActualInputs()
              setActualOutputs()
              setActualStateTexts()
              break
            }
            case 'SetValues': {
              const notSetValues = JSON.parse(response.Value!) as string[]
              if (notSetValues.length > 0) {
                notify(`Nepodarilo sa nastaviť: ${notSetValues.join(', ')}`, 'error')
              } else {
                // notify('Hodnoty boli úspešne nastavené', 'success')
                if (store.isStartNavazovanieModalOpened) {
                  store.isNavazovanieInitSuccess = true
                }
              }
              break
            }
            case 'SetControlValues': {
              const notSetValues = JSON.parse(response.Value!) as string[]
              if (notSetValues.length > 0) {
                notify(`Nepodarilo sa nastaviť: ${notSetValues.join(', ')}`, 'error')
              } else {
                // notify('Hodnoty boli úspešne nastavené', 'success')
              }
              break
            }
            case 'SetZeroing': {
              const responseValue = JSON.parse(response.Value!) as boolean
              if (!responseValue) {
                notify('Nulovanie sa nepodarilo', 'error')
              }
              break
            }
            case 'GetZariadenia': {
              store.zariadenia = (
                JSON.parse(response.Value!) as AlyaVaha.Models.IZariadenie[]
              ).sort((a, b) => a.NazovZariadenia!.localeCompare(b.NazovZariadenia!))
              break
            }
            case 'GetUzivatelia': {
              store.uzivatelia = JSON.parse(response.Value!) as AlyaVaha.Models.IUzivatel[]
              store.aktivniUzivatelia = store.uzivatelia
                .filter((m) => m.JeAktivny)
                .sort((a, b) => a.Login!.localeCompare(b.Login!))
              store.uzivateliaLoading = false
              break
            }
            case 'GetMaterialy': {
              store.materialy = JSON.parse(response.Value!) as AlyaVaha.Models.IMaterial[]
              store.aktivneMaterialy = store.materialy
                .filter((m) => m.JeAktivny)
                .sort((a, b) => a.NazovMaterialu!.localeCompare(b.NazovMaterialu!))
              store.materialyLoading = false
              break
            }
            case 'GetZasobniky': {
              store.zasobniky = JSON.parse(response.Value!) as AlyaVaha.Models.IZasobnik[]
              store.zasobnikyDoVahy = store.zasobniky
                .filter((z) => z.CestaDoVahy)
                .sort((a, b) => a.NazovZasobnika!.localeCompare(b.NazovZasobnika!))
              store.zasobnikyZVahy = store.zasobniky
                .filter((z) => z.CestaZVahy)
                .sort((a, b) => a.NazovZasobnika!.localeCompare(b.NazovZasobnika!))
              store.zasobnikyLoading = false
              break
            }
            case 'GetNavazovania': {
              const responseValue = JSON.parse(response.Value!)
              store.navazovaniaData = responseValue
              break
            }
            case 'GetExportNavazovania': {
              const responseValue = JSON.parse(response.Value!)
              store.navazovaniaDataExport = responseValue
              break
            }
            case 'GetStatistiky': {
              const statistikyResponse = JSON.parse(response.Value!)
              store.statistiky = statistikyResponse.summary
              if (store.statistiky) {
                store.statistiky.unshift(statistikyResponse.totalCount)
              }
              break
            }
            default: {
              if (
                response.Command.startsWith('Add') ||
                response.Command.startsWith('Update') ||
                response.Command.startsWith('Delete')
              ) {
                const operationResult = JSON.parse(response.Value!) as AlyaVaha.IOperationResult
                const message = operationResult.Message
                const success = operationResult.Success
                notify(message, success ? 'info' : 'error')
              }
              if (
                response.Command == 'AddMaterial' ||
                response.Command == 'UpdateMaterial' ||
                response.Command == 'DeleteMaterial'
              ) {
                sendCommand('GetMaterialy')
              } else if (
                response.Command == 'AddZasobnik' ||
                response.Command == 'UpdateZasobnik' ||
                response.Command == 'DeleteZasobnik'
              ) {
                sendCommand('GetZasobniky')
              } else if (
                response.Command == 'AddZariadenie' ||
                response.Command == 'UpdateZariadenie' ||
                response.Command == 'DeleteZariadenie'
              ) {
                sendCommand('GetZariadenia')
              } else if (
                response.Command == 'AddUzivatel' ||
                response.Command == 'UpdateUzivatel' ||
                response.Command == 'DeleteUzivatel'
              ) {
                sendCommand('GetUzivatelia')
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

async function getLoggedInUser() {
  sendCommand('GetLoggedInUser')
}

async function getAllData() {
  sendCommand('GetZariadenia')
  sendCommand('GetMaterialy')
  sendCommand('GetZasobniky')
  sendCommand('GetUzivatelia')
}

async function setActualInputs() {
  const vstupy = store.actualData.DigitalneVstupy
  if (vstupy) {
    for (let i = 0; i < vstupy.length; i++) {
      switch (i + 1) {
        case VahaAPI.ProgramVahaDigitalInputNumber.HornaKlapkaOtvorena:
          store.actualInputs.HornaKlapkaOtvorena = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.HornaKlapkaZatvorena:
          store.actualInputs.HornaKlapkaZatvorena = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.DolnaKlapkaOtvorena:
          store.actualInputs.DolnaKlapkaOtvorena = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.DolnaKlapkaZatvorena:
          store.actualInputs.DolnaKlapkaZatvorena = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.RucnyRezim:
          store.actualInputs.RucnyRezim = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.Napajanie24V:
          store.actualInputs.Napajanie24V = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.BlokovanieVypustenia:
          store.actualInputs.BlokovanieVypustenia = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.StopNavazovania:
          store.actualInputs.StopNavazovania = vstupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalInputNumber.HornaKlapkaStred:
          store.actualInputs.HornaKlapkaStred = vstupy[i] == '1'
          break
      }
    }
  }
}

async function setActualOutputs() {
  const vystupy = store.actualData.DigitalneVystupy
  if (vystupy) {
    for (let i = 0; i < vystupy.length; i++) {
      switch (i + 1) {
        case VahaAPI.ProgramVahaDigitalOutputNumber.HornuKlapkuOtvor:
          store.actualOutputs.HornuKlapkuOtvor = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.DolnuKlapkuOtvor:
          store.actualOutputs.DolnuKlapkuOtvor = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Sirena:
          store.actualOutputs.Sirena = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.NavazovanieBezi:
          store.actualOutputs.NavazovanieBezi = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Porucha:
          store.actualOutputs.Porucha = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Vibrator:
          store.actualOutputs.Vibrator = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Odfuk:
          store.actualOutputs.Odfuk = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Blokovanie1:
          store.actualOutputs.Blokovanie1 = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.Blokovanie2:
          store.actualOutputs.Blokovanie2 = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.HornuKlapkuNaStred:
          store.actualOutputs.HornuKlapkuNaStred = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.HornuKlapkuZatvor:
          store.actualOutputs.HornuKlapkuZatvor = vystupy[i] == '1'
          break
        case VahaAPI.ProgramVahaDigitalOutputNumber.DolnuKlapkuZatvor:
          store.actualOutputs.DolnuKlapkuZatvor = vystupy[i] == '1'
          break
      }
    }
  }
}

async function changeDigitalOutput(outputProperty: string) {
  let vystupy = store.actualData.DigitalneVystupy
  if (vystupy) {
    const outputNumber =
      VahaAPI.ProgramVahaDigitalOutputNumber[
        outputProperty as keyof typeof VahaAPI.ProgramVahaDigitalOutputNumber
      ]
    const index = outputNumber - 1
    const newValue = vystupy[index] == '1' ? '0' : '1'
    // Replace a char in a string at position index by newValue
    vystupy = vystupy.substring(0, index) + newValue + vystupy.substring(index + 1)
    sendCommand('SetValues', { DigitalneVystupy: vystupy })
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

export { initCommandHandler, sendCommand, getAllData, getLoggedInUser, changeDigitalOutput }
