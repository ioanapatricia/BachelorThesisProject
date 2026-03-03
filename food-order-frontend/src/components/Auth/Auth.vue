<template>
  <b-modal
    id="modal-center"
    size="xl"
    centered
    title="Authentication"
    :hide-footer="true"
    class="modalFont"
    header-bg-variant="dark"
    header-text-variant="light"
  >
    <b-container>
      <b-row>
        <b-col md align-items="center">
          <p class="h3">Login</p>
          <b-form class="my-3" @submit.stop.prevent>
            <label for="loginUsername" class="mb-2">Username</label>
            <b-form-input
              v-model="login.username"
              required
              :state="loginUsernameValidation"
              id="loginUsername"
            ></b-form-input>
            <b-form-invalid-feedback :state="loginUsernameValidation">
              Username is required.
            </b-form-invalid-feedback>

            <label for="loginPassword" class="mb-2 mt-3">Password</label>
            <b-form-input
              v-model="login.password"
              required
              type="password"
              :state="loginPasswordValidation"
              id="loginPassword"
            ></b-form-input>
            <b-form-invalid-feedback :state="loginPasswordValidation">
              Password is required.
            </b-form-invalid-feedback>
            <p style="color: red" class="mt-3" v-if="showWrongCredentials">
              Username or password is incorrect.
            </p>
            <b-button
              flat
              variant="dark"
              squared
              class="float-right mt-3"
              @click="onLogin()"
            >
              Login and place order
            </b-button>
          </b-form>
        </b-col>
        <!-- REGISTER -->
        <b-col md style="border-left: 1px solid #e7e7e7">
          <!-- USERNAME -->
          <p class="h3">Your Personal Info:</p>
          <b-form class="my-3" @submit.stop.prevent>
            <label for="registerUsername" class="mb-2">Username</label>
            <b-form-input
              v-model="register.username"
              required
              :state="registerUsernameValidation"
              id="registerUsername"
            ></b-form-input>
            <b-form-invalid-feedback :state="registerUsernameValidation">
              Username requires min 5 characters.
            </b-form-invalid-feedback>
            <p
              style="color: red"
              class="mt-3"
              v-if="showWrongUsernameCredentials"
            >
              Username is already taken.
            </p>

            <!-- PASSWORD AND CHECK PASSWORD -->
            <b-row>
              <b-col cols="6">
                <label for="registerPassword" class="mb-2 mt-3">
                  Password
                </label>
                <b-form-input
                  v-model="register.password"
                  required
                  type="password"
                  :state="registerPasswordValidation"
                  id="registerPassword"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerPasswordValidation">
                  Password requires min 5 characters.
                </b-form-invalid-feedback>
              </b-col>
              <b-col cols="6">
                <label for="registerConfirmPassword" class="mb-2 mt-3">
                  Confirm Password
                </label>
                <b-form-input
                  v-model="register.confirmPassword"
                  required
                  type="password"
                  :state="registerConfirmPasswordValidation"
                  id="registerConfirmPassword"
                ></b-form-input>
                <b-form-invalid-feedback
                  :state="registerConfirmPasswordValidation"
                >
                  Passwords must match.
                </b-form-invalid-feedback>
              </b-col>
            </b-row>

            <!-- FIRSTNAME AND LASTNAME -->
            <b-row>
              <b-col cols="6">
                <label for="registerFirstname" class="mb-2 mt-3">
                  Firstname
                </label>
                <b-form-input
                  v-model="register.firstname"
                  required
                  :state="registerFirstnameValidation"
                  id="registerFirstname"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerFirstnameValidation">
                  Firstname requires min 3 characters.
                </b-form-invalid-feedback>
              </b-col>
              <b-col cols="6">
                <label for="registerLastname" class="mb-2 mt-3">
                  Lastname
                </label>
                <b-form-input
                  v-model="register.lastname"
                  required
                  :state="registerLastnameValidation"
                  id="registerLastname"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerLastnameValidation">
                  Lastname requires min 3 characters.
                </b-form-invalid-feedback>
              </b-col>
            </b-row>

            <!-- EMAIL AND NUMBER -->
            <b-row>
              <b-col cols="6">
                <label for="registerEmail" class="mb-2 mt-3">
                  Email
                </label>
                <b-form-input
                  v-model="register.email"
                  required
                  :state="registerEmailValidation"
                  id="registerEmail"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerEmailValidation">
                  Email must be valid.
                </b-form-invalid-feedback>
              </b-col>
              <b-col cols="6">
                <label for="registerPhoneNumber" class="mb-2 mt-3">
                  Phone Number
                </label>
                <b-form-input
                  v-model="register.phoneNumber"
                  required
                  :state="registerPhoneNumberValidation"
                  id="registerPhoneNumber"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerPhoneNumberValidation">
                  Must be a valid phone number.
                </b-form-invalid-feedback>
              </b-col>
            </b-row>

            <p class="h3 mt-4">Where should we deliver:</p>
            <!-- CITY AND COUNTY -->
            <b-row>
              <b-col cols="6">
                <label for="registerCity" class="mb-2 mt-3">
                  City
                </label>
                <b-form-input
                  v-model="register.address.city"
                  required
                  :state="registerCityValidation"
                  id="registerCity"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerCityValidation">
                  City is required.
                </b-form-invalid-feedback>
              </b-col>
              <b-col cols="6">
                <label for="registerCounty" class="mb-2 mt-3">
                  County
                </label>
                <b-form-input
                  v-model="register.address.county"
                  required
                  :state="registerCountyValidation"
                  id="registerCounty"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerCountyValidation">
                  County is required.
                </b-form-invalid-feedback>
              </b-col>
            </b-row>

            <!-- STREET AND STREET NUMBER -->
            <b-row>
              <b-col cols="6">
                <label for="registerStreet" class="mb-2 mt-3">
                  Street
                </label>
                <b-form-input
                  v-model="register.address.street"
                  required
                  :state="registerStreetValidation"
                  id="registerStreet"
                ></b-form-input>
                <b-form-invalid-feedback :state="registerStreetValidation">
                  Street is required.
                </b-form-invalid-feedback>
              </b-col>
              <b-col cols="6">
                <label for="registerStreetNumber" class="mb-2 mt-3">
                  Street Number
                </label>
                <b-form-input
                  v-model="register.address.streetNumber"
                  required
                  :state="registerStreetNumberValidation"
                  id="registerStreetNumber"
                ></b-form-input>
                <b-form-invalid-feedback
                  :state="registerStreetNumberValidation"
                >
                  A valid street number is required.
                </b-form-invalid-feedback>
              </b-col>
            </b-row>

            <!-- BUILDING AND FLOOR -->
            <b-row>
              <b-col cols="6">
                <label for="registerBuilding" class="mb-2 mt-3">
                  Building
                </label>
                <b-form-input
                  v-model="register.address.building"
                  required
                  id="registerBuilding"
                ></b-form-input>
              </b-col>
              <b-col cols="6">
                <label for="registerFloor" class="mb-2 mt-3">
                  Floor
                </label>
                <b-form-input
                  v-model="register.address.floor"
                  required
                  id="registerFloor"
                ></b-form-input>
              </b-col>
            </b-row>

            <!-- ENTRANCE AND APARTMENT NUMBER -->
            <b-row>
              <b-col cols="6">
                <label for="registerEntrance" class="mb-2 mt-3">
                  Entrance
                </label>
                <b-form-input
                  v-model="register.address.entrance"
                  required
                  id="registerEntrance"
                ></b-form-input>
              </b-col>
              <b-col cols="6">
                <label for="registerApartmentNumber" class="mb-2 mt-3">
                  Apartment Number
                </label>
                <b-form-input
                  v-model="register.address.apartmentNumber"
                  required
                  id="registerApartmentNumber"
                ></b-form-input>
              </b-col>
            </b-row>

            <!-- SECTOR -->
            <b-row>
              <b-col cols="6">
                <label for="registerSector" class="mb-2 mt-3">
                  Sector
                </label>
                <b-form-input
                  v-model="register.address.sector"
                  required
                  id="registerSector"
                ></b-form-input>
              </b-col>
            </b-row>

            <b-button
              flat
              variant="dark"
              squared
              class="float-right mt-3"
              @click="onRegister()"
            >
              Register and place order
            </b-button>
          </b-form>
        </b-col>
      </b-row>
    </b-container>
    <b-overlay :show="overlay" no-wrap> </b-overlay>
  </b-modal>
</template>

<script>
import { login, register, createOrder } from '../../apiService/apiService'
import ValidationMixin from './validation'
export default {
  name: 'Auth',
  props: ['order'],
  mixins: [ValidationMixin],
  data: () => ({
    overlay: false,
    login: {
      username: null,
      password: null,
    },
    register: {
      username: null,
      password: null,
      confirmPassword: null,
      firstname: null,
      lastname: null,
      email: null,
      phoneNumber: null,
      address: {
        city: null,
        county: null,
        street: null,
        streetNumber: null,
        building: null,
        floor: null,
        entrance: null,
        apartmentNumber: null,
        sector: null,
      },
    },
    showWrongCredentials: false,
    showWrongUsernameCredentials: false,
  }),

  methods: {
    async onLogin() {
      if (!this.isLoginFormValid()) {
        return
      }
      this.overlay = true
      this.showWrongCredentials = false

      await login(this.login)
        .then(loginResponse => {
          if (loginResponse.status == 200) {
            this.$store.dispatch('login', loginResponse.data)
            return createOrder(this.order)
          }
        })
        .then(createOrderResponse => {
          console.log(createOrderResponse)
          this.$store.dispatch('deleteAllCartProducts')
          this.$router.push('/orderCompleted')
        })
        .catch(error => {
          console.log('statusuuu wai', error.response.status)
          if (error.response.status == 401) {
            this.showWrongCredentials = true
            this.overlay = false
          } else {
            this.$router.push('/error')
          }
        })
    },
    async onRegister() {
      if (!this.isRegisterFormValid()) {
        return
      }

      this.overlay = true
      this.showWrongCredentials = false
      console.log('Register:', this.register)
      await register(this.register)
        .then(registerResponse => {
          if (registerResponse.status == 200) {
            const intermediateLogin = {
              username: this.register.username,
              password: this.register.password,
            }
            return login(intermediateLogin)
          }
        })
        .then(loginResponse => {
          if (loginResponse.status == 200) {
            this.$store.dispatch('login', loginResponse.data)
            return createOrder(this.order)
          }
        })
        .then(orderCreationResponse => {
          if (orderCreationResponse.status == 201) {
            this.$store.dispatch('deleteAllCartProducts')
            this.$router.push('/orderCompleted')
          }
        })
        .catch(error => {
          if (error.response.status == 400) {
            this.showWrongUsernameCredentials = true
            this.overlay = false
          } else {
            this.$router.push('/error')
          }
        })
    },
  },
}
</script>

<style></style>
