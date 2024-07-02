const defaultUser = {
  email: 'Admin'
}

export default {
  _user: defaultUser,
  loggedIn() {
    return !!this._user
  },

  async logIn(email, password) {
    try {
      // Send request
      if (email !== 'admin' || password !== 'alya123456') throw new Error()

      this._user = { ...defaultUser, email }

      return {
        isOk: true,
        data: this._user
      }
    } catch {
      return {
        isOk: false,
        message: 'Nesprávne prihlasovacie údaje'
      }
    }
  },

  async logOut() {
    this._user = null
  },

  async getUser() {
    try {
      // Send request

      return {
        isOk: true,
        data: this._user
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
