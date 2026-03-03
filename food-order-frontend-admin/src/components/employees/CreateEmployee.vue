<template>
  <v-card class="ma-5">
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
    <snack-bar :snackbar="snackbar" />
    <v-card-text>
      <p class="text-h5 grey--text">New Employee</p>
      <v-form ref="form" v-model="valid" lazy-validation>
        <v-text-field
          v-model="employee.username"
          label="Username"
          :rules="usernameRules"
          prepend-icon="mdi-alien"
          required
        />
        <v-text-field
          v-model="employee.firstName"
          label="First Name"
          :rules="firstNameRules"
          prepend-icon="mdi-account"
          required
        />
        <v-text-field
          v-model="employee.lastName"
          label="Last Name"
          :rules="lastNameRules"
          prepend-icon="mdi-account-outline"
          required
        />
        <v-text-field
          v-model="employee.password"
          label="Password"
          :rules="passwordRules"
          prepend-icon="mdi-lock"
          required
          type="password"
        />
        <v-text-field
          v-model="employee.email"
          label="Email"
          :rules="emailRules"
          prepend-icon="mdi-at"
          required
        />
        <v-text-field
          v-model="employee.phoneNumber"
          label="Phone Number"
          :rules="phoneNumberRules"
          prepend-icon="mdi-phone"
          required
        />

        <v-select
          v-model="employee.roleId"
          :items="roles"
          item-value="id"
          item-text="name"
          :rules="roleIdRules"
          prepend-icon="mdi-pound"
          label="Role"
          required
        />

        <h2 class="mt-2">Address</h2>

        <v-text-field
          v-model="employee.address.city"
          label="City"
          :rules="cityRules"
          prepend-icon="mdi-city-variant"
          required
        />
        <v-text-field
          v-model="employee.address.county"
          label="County"
          :rules="countyRules"
          prepend-icon="mdi-city"
          required
        />

        <v-text-field
          v-model="employee.address.street"
          label="Street"
          :rules="streetRules"
          prepend-icon="mdi-road-variant"
          required
        />
        <v-text-field
          v-model="employee.address.streetNumber"
          label="Street Number"
          :rules="streetNumberRules"
          prepend-icon="mdi-counter"
          type="number"
          required
        />

        <v-row>
          <v-col>
            <v-btn
              :disabled="!valid"
              color="success"
              class="mr-4"
              @click="onSubmit"
            >
              Submit
            </v-btn>
          </v-col>
          <v-col class="text-right">
            <v-btn color="error" class="mr-4" to="/products">
              Back
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script>
import SnackBar from '../common/SnackBar'
import { createEmployee } from '../../apiService/apiService'
export default {
  components: { SnackBar },
  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    overlay: false,
    valid: true,
    employee: {
      username: null,
      firstName: null,
      lastName: null,
      password: null,
      email: null,
      phoneNumber: null,
      address: {
        city: null,
        county: null,
        street: null,
        streetNumber: null,
      },
      roleId: null,
    },
    usernameRules: [
      v => !!v || 'Username is required',
      v =>
        (v && v.length >= 3 && v.length <= 30) ||
        'Username must be between 3 and 30 characters',
    ],
    firstNameRules: [
      v => !!v || 'First name is required',
      v =>
        (v && v.length >= 3 && v.length <= 30) ||
        'First name must be between 3 and 30 characters',
    ],
    lastNameRules: [
      v => !!v || 'Last name is required',
      v =>
        (v && v.length >= 3 && v.length <= 30) ||
        'Last name must be between 3 and 30 characters',
    ],
    passwordRules: [
      v => !!v || 'Password is required',
      v => (v && v.length >= 5) || 'Password must be at least 5 characters',
    ],

    emailRules: [
      v => !!v || 'Email is required',
      v => /\S+@\S+\.\S+/.test(v) || 'Email is not valid',
    ],

    phoneNumberRules: [
      v => !!v || 'Phone number is required',
      v => (v && v.length >= 10) || 'Phone number be at least 10 characters',
    ],

    roleIdRules: [v => !!v || 'Role is required'],

    cityRules: [v => !!v || 'City is required'],
    countyRules: [v => !!v || 'County is required'],
    streetRules: [v => !!v || 'Street is required'],
    streetNumberRules: [v => !!v || 'Street number is required'],
  }),

  created() {
    if (this.$store.getters.getRoles.length == 0) {
      this.$store.dispatch('setRolesWithApiDataAction')
    }
  },

  computed: {
    roles() {
      return this.$store.getters.getRoles
    },
  },
  methods: {
    async onSubmit() {
      if (!this.$refs.form.validate()) {
        return
      }

      this.overlay = true
      await createEmployee(this.employee)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            this.initSnackbarSuccess()
            this.$store.dispatch('setEmployeesWithApiDataAction')
            setTimeout(() => {
              this.overlay = false
              this.$router.push(`/viewEditEmployee/${response.data.id}`)
            }, 1000)
          }
        })
        .catch(error => {
          this.overlay = false
          this.initSnackbarFailure(error.response.data)
        })
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Employee created successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to create employee, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },
  },
}
</script>

<style></style>
