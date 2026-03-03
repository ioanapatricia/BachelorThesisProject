export default {
  methods: {
    isLoginFormValid() {
      let isValid = true

      for (let [key, value] of Object.entries(this.login)) {
        if (value == null || value == '') {
          this.login[`${key}`] = ''
          isValid = false
        }
      }
      return isValid
    },

    isRegisterFormValid() {
      let isValid = true

      for (let [key, value] of Object.entries(this.register)) {
        if (key != 'address') {
          if (value == null) {
            this.register[`${key}`] = ''
            isValid = false
          }

          if (value == '') {
            isValid = false
          }
        }
      }

      const addressFieldsForValidation = ['city','county','street','streetNumber'];

      for (let [key, value] of Object.entries(this.register.address)) {
        if (addressFieldsForValidation.includes(key)) {
          if (value == null) {
            this.register.address[`${key}`] = ''
            isValid = false
          }

          if (value == '') {
            isValid = false
          }
        }
      }

      return isValid
    }
  },

  computed: {
    loginUsernameValidation() {
      if (this.login.username == null) {
        return null
      }
      if (this.login.username == '') {
        return false
      }
      return true
    },
    loginPasswordValidation() {
      if (this.login.password == null) {
        return null
      }
      if (this.login.password == '') {
        return false
      }
      return true
    },
    registerUsernameValidation() {
      if (this.register.username == null) {
        return null
      }
      if (this.register.username == '' || this.register.username.length < 5) {
        return false
      }
      return true
    },
    registerPasswordValidation() {
      if (this.register.password == null) {
        return null
      }
      if (this.register.password == '' || this.register.password.length < 5) {
        return false
      }
      return true
    },
    registerConfirmPasswordValidation() {
      if (this.register.confirmPassword == null) {
        return null
      }
      if (this.register.confirmPassword != this.register.password) {
        return false
      }
      if (this.register.confirmPassword == '') {
        return false
      }
      return true
    },
    registerFirstnameValidation() {
      if (this.register.firstname == null) {
        return null
      }
      if (this.register.firstname == '' || this.register.firstname.length < 3) {
        return false
      }
      return true
    },
    registerLastnameValidation() {
      if (this.register.lastname == null) {
        return null
      }
      if (this.register.lastname == '' || this.register.lastname.length < 3) {
        return false
      }
      return true
    },
    registerEmailValidation() {
      if (this.register.email == null) {
        return null
      }
      const re = /\S+@\S+\.\S+/
      if (!re.test(this.register.email)) {
        return false
      }
      return true
    },
    registerPhoneNumberValidation() {
      if (this.register.phoneNumber == null) {
        return null
      }
      if (
        this.register.phoneNumber == '' ||
        this.register.phoneNumber.length != 10 ||
        typeof +this.register.phoneNumber != 'number'
      ) {
        return false
      }
      return true
    },
    registerCityValidation() {
      if (this.register.address.city == null) {
        return null
      }
      if (
        this.register.address.city == '' ||
        this.register.address.city.length < 3
      ) {
        return false
      }
      return true
    },
    registerCountyValidation() {
      if (this.register.address.county == null) {
        return null
      }
      if (
        this.register.address.county == '' ||
        this.register.address.county.length < 3
      ) {
        return false
      }
      return true
    },

    registerStreetValidation() {
      if (this.register.address.street == null) {
        return null
      }
      if (
        this.register.address.street == '' ||
        this.register.address.street.length < 3
      ) {
        return false
      }
      return true
    },

    registerStreetNumberValidation() {
      if (this.register.address.streetNumber == null) {
        return null
      }
      if (this.register.address.streetNumber == '') {
        return false
      }
      if ( !/^-?\d+$/.test(this.register.address.streetNumber)) {
        return false
      }

      return true
    },
  },
}
