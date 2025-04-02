import { formatTime, parseCustomDate } from '@/utils/helpers'

import store from '@/store'
import * as VahaAPI from '@/types/vahaTypes'
import { sendCommand } from './commandHandler'

let timeoutData = null // as ReturnType<typeof setTimeout> | undefined
let vahaCheckInterval = null
let vahaErrorTimeout = null

let lastBruttoVaha = 0
let isNepribudaMonitoring = false
let isNeodbudaMonitoring = false

// sekundovy interval
setInterval(() => {
  if (store.actualData.StavNavazovania === VahaAPI.StavNavazovania.NavazovanieBezi) {
    computeTimerNavazovanie()
  }

  if (
    store.actualData.StavRiadeniaNavazovania === VahaAPI.StavRiadeniaNavazovania.NavazujeSa &&
    store.nastavenia.SirenaPriNepribudani
  ) {
    if (!isNepribudaMonitoring) {
      startVahaMonitoring(
        store.nastavenia.VahaSirenyPriNepribudani,
        store.nastavenia.CasSirenyPriNepribudani
      )
      isNepribudaMonitoring = true
    }
  } else {
    if (isNepribudaMonitoring) {
      stopWeightMonitoring()
      isNepribudaMonitoring = false
    }
  }

  if (
    store.actualData.StavRiadeniaNavazovania ===
      VahaAPI.StavRiadeniaNavazovania.PrebiehaVyprazdnovanie &&
    store.nastavenia.SirenaPriNeodbudani
  ) {
    if (!isNeodbudaMonitoring) {
      startVahaMonitoring(
        store.nastavenia.VahaSirenyPriNeodbudani,
        store.nastavenia.CasSirenyPriNeodbudani
      )
      isNeodbudaMonitoring = true
    }
  } else {
    if (isNeodbudaMonitoring) {
      stopWeightMonitoring()
      isNeodbudaMonitoring = false
    }
  }
}, 1000)

function afterTimeout() {
  if (!store.navazovaniaLoading && !store.statistikyLoading) {
    store.connected = false
  } else {
    timeoutData = setTimeout(afterTimeout, 3000)
  }
}

function restartTimeoutData() {
  if (timeoutData) {
    clearTimeout(timeoutData)
  }
  timeoutData = setTimeout(afterTimeout, 3000)
}

function computeTimerNavazovanie() {
  const now = new Date()
  const startDatetime = parseCustomDate(store.actualData.DatumCasStartuNavazovania)
  const timeDiff = Math.floor((now.getTime() - startDatetime.getTime()) / 1000)
  store.timerNavazovanie = formatTime(timeDiff)
}

function startVahaMonitoring(vahaSirena, casSirena) {
  // Clear any existing interval or timeout
  stopWeightMonitoring()

  // Initialize the last weight
  lastBruttoVaha = store.previousData?.BruttoVaha || 0

  // Start monitoring the weight every second
  vahaCheckInterval = setInterval(() => {
    const currentWeight = store.actualData?.BruttoVaha || 0

    // Check if the weight has increased by at least entered amount
    if (Math.abs(currentWeight - lastBruttoVaha) >= vahaSirena) {
      // Reset the timeout if the weight has increased sufficiently
      if (vahaErrorTimeout) {
        clearTimeout(vahaErrorTimeout)
        vahaErrorTimeout = null
      }

      // Update the last weight
      lastBruttoVaha = currentWeight
    } else {
      // If no sufficient increase, start or reset the error timeout
      if (!vahaErrorTimeout) {
        vahaErrorTimeout = setTimeout(() => {
          sendCommand('SetValues', {
            StavSireny: VahaAPI.StavSirenyPovel.ZapniSirenu
          })
        }, casSirena * 1000) // Convert seconds to milliseconds
      }
    }
  }, 1000) // Check every second
}

function stopWeightMonitoring() {
  // Clear the interval and timeout when monitoring stops
  if (vahaCheckInterval) {
    clearInterval(vahaCheckInterval)
    vahaCheckInterval = null
  }
  if (vahaErrorTimeout) {
    clearTimeout(vahaErrorTimeout)
    vahaErrorTimeout = null
  }
}

// const afterTimeoutNepribuda = () => {
//   if (store.actualData.StavRiadeniaNavazovania === VahaAPI.StavRiadeniaNavazovania.NavazujeSa) {
//     sendCommand('SetValues', {
//       StavSireny: VahaAPI.StavSirenyPovel.ZapniSirenu
//     })
//     console.log('Zapni sirenu nepribuda')
//   }
// }

// const afterTimeoutNeodbuda = () => {
//   if (
//     store.actualData.StavRiadeniaNavazovania ===
//     VahaAPI.StavRiadeniaNavazovania.PrebiehaVyprazdnovanie
//   ) {
//     sendCommand('SetValues', {
//       StavSireny: VahaAPI.StavSirenyPovel.ZapniSirenu
//     })
//     console.log('Zapni sirenu neodbuda')
//   }
// }
//
// async function checkVahaNepribuda() {
//   if (store.previousData?.BruttoVaha === null) return

//   if (
//     store.actualData.StavRiadeniaNavazovania === VahaAPI.StavRiadeniaNavazovania.NavazujeSa &&
//     store.nastavenia.SirenaPriNepribudani &&
//     store.actualData.BruttoVaha === store.previousData?.BruttoVaha
//   ) {
//     if (!timeoutNepribuda) {
//       timeoutNepribuda = setTimeout(
//         afterTimeoutNepribuda,
//         store.nastavenia.CasSirenyPriNepribudani * 1000
//       )
//     }
//     return
//   }

//   if (timeoutNepribuda) {
//     clearTimeout(timeoutNepribuda)
//     timeoutNepribuda = undefined
//   }
// }

// async function checkVahaNeodbuda() {
//   if (store.previousData?.BruttoVaha === null) return

//   if (
//     store.actualData.StavRiadeniaNavazovania ===
//       VahaAPI.StavRiadeniaNavazovania.PrebiehaVyprazdnovanie &&
//     store.nastavenia.SirenaPriNeodbudani &&
//     store.actualData.BruttoVaha === store.previousData?.BruttoVaha
//   ) {
//     if (!timeoutNeodbuda) {
//       timeoutNeodbuda = setTimeout(
//         afterTimeoutNeodbuda,
//         store.nastavenia.CasSirenyPriNeodbudani * 1000
//       )
//     }
//     return
//   }

//   if (timeoutNeodbuda) {
//     clearTimeout(timeoutNeodbuda)
//     timeoutNeodbuda = undefined
//   }
// }

export { restartTimeoutData }
