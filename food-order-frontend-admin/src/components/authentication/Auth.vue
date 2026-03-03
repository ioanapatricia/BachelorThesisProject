<template>
  <v-row align="center" justify="center">
    <v-col align="center" justify="center">
      <v-card max-width="550" dark class="grey darken-3">
        <v-card-title>
          Authentication
        </v-card-title>
        <v-card-text>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="loginData.username"
              :rules="usernameRule"
              label="Username"
              prepend-icon="mdi-account"
              required
              :disabled="loading"
            />

            <v-text-field
              v-model="loginData.password"
              :rules="passwordRule"
              type="password"
              label="Password"
              prepend-icon="mdi-lock"
              required
              :disabled="loading"
            />
          </v-form>
          <v-btn block @click="onSubmit()" :loading="loading" class=" mt-3"
            >Login</v-btn
          >
        </v-card-text>
      </v-card>
    </v-col>
    <snack-bar :snackbar="snackbar" />
  </v-row>
</template>

<script>
import SnackBar from '../common/SnackBar'

import { login } from '../../apiService/apiService'
import { isUserValid } from '../../helpers/authExtensions.js'
import VueJwtDecode from 'vue-jwt-decode'

export default {
  name: 'Categories',
  components: { SnackBar },
  data: () => ({
    loading: false,
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    loginData: {
      username: null,
      password: null,
    },
    valid: true,
    usernameRule: [v => !!v || 'Username is required'],
    passwordRule: [v => !!v || 'Password is required'],
  }),
  methods: {
    async onSubmit() {
      this.$refs.form.validate()
      if (!this.$refs.form.validate()) {
        return
      }
      this.loading = true

      await login(this.loginData)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            const loggedUser = response.data

            if (!isUserValid(loggedUser)) {
              this.initSnackbarFailure(
                'You are not authorized to visit this page',
              )
              return
            }

            loggedUser['role'] = VueJwtDecode.decode(loggedUser.token).role

            this.$store.dispatch('setLoggedUserAction', loggedUser).then(() => {
              this.$router.push(`/`)
            })

            // JSON.parse(localStorage.getItem('loggedUser')),
          }
        })

        .catch(() => {
          this.initSnackbarFailure(
            'Login failed: incorrect username or password',
          )
        })

        .finally(() => {
          this.loading = false
        })
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: errorMessage,
        color: 'error',
        show: true,
      }
    },
  },
}
</script>

<style></style>
