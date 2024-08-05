import { watch } from 'vue'
import store from '@/store'
import { sendCommand } from './commandHandler'

export default {
  loggedIn() {
    return !!store.user
  },

  async logIn(email, password) {
    return new Promise((resolve) => {
      try {
        const stop = watch(
          () => store.isUserLoggedIn,
          () => {
            stop()
            if (store.isUserLoggedIn) {
              resolve({
                isOk: true,
                data: store.user
              })
            } else {
              store.isUserLoggedIn = null
              resolve({
                isOk: false
              })
            }
          }
        )
        sendCommand('Login', { login: email, heslo: password })
      } catch (error) {
        resolve(error)
      }
    })
  },

  async logOut() {
    store.isUserLoggedIn = false
    store.user = null
  },

  async getUser() {
    try {
      // Send request

      return {
        isOk: true,
        data: store.user
      }
    } catch {
      return {
        isOk: false
      }
    }
  },

  async resetPassword(email) {
    try {
      // Send request
      console.log(email)

      return {
        isOk: true
      }
    } catch {
      return {
        isOk: false,
        message: 'Chyba pri resetovaní hesla'
      }
    }
  },

  async changePassword(email, recoveryCode) {
    try {
      // Send request
      console.log(email, recoveryCode)

      return {
        isOk: true
      }
    } catch {
      return {
        isOk: false,
        message: 'Chyba pri zmene hesla'
      }
    }
  },

  async createAccount(email, password) {
    try {
      // Send request
      console.log(email, password)

      return {
        isOk: true
      }
    } catch {
      return {
        isOk: false,
        message: 'Chyba pri vytváraní účtu'
      }
    }
  }
}
