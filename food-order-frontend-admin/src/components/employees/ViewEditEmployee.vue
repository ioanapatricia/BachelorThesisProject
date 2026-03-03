<template>
  <v-card class="ma-5" :loading="loading">
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
    <snack-bar :snackbar="snackbar" />
    <v-card-text>
      <p class="text-h5 grey--text">
        {{ employee ? `${employee.firstName} ${employee.lastName}` : '' }}
      </p>
      <v-form
        ref="form"
        v-model="valid"
        lazy-validation
        v-if="employeeForUpdate"
      >
        <v-text-field
          v-model="employeeForUpdate.username"
          label="Username"
          :rules="usernameRules"
          prepend-icon="mdi-alien"
          required
          :disabled="viewOnlyMode"
        />
        <v-text-field
          v-model="employeeForUpdate.firstName"
          label="First Name"
          :rules="firstNameRules"
          prepend-icon="mdi-account"
          required
          :disabled="viewOnlyMode"
        />
        <v-text-field
          v-model="employeeForUpdate.lastName"
          label="Last Name"
          :rules="lastNameRules"
          prepend-icon="mdi-account-outline"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="employeeForUpdate.email"
          label="Email"
          :rules="emailRules"
          prepend-icon="mdi-at"
          required
          :disabled="viewOnlyMode"
        />
        <v-text-field
          v-model="employeeForUpdate.phoneNumber"
          label="Phone Number"
          :rules="phoneNumberRules"
          prepend-icon="mdi-phone"
          required
          :disabled="viewOnlyMode"
        />

        <v-select
          v-model="employeeForUpdate.role.id"
          :items="roles"
          item-value="id"
          item-text="name"
          :rules="roleIdRules"
          prepend-icon="mdi-pound"
          label="Role"
          required
          :disabled="viewOnlyMode"
        />

        <h2 class="mt-1 mb-3">Address</h2>

        <v-text-field
          v-model="employeeForUpdate.address.city"
          label="City"
          :rules="cityRules"
          prepend-icon="mdi-city-variant"
          required
          :disabled="viewOnlyMode"
        />
        <v-text-field
          v-model="employeeForUpdate.address.county"
          label="County"
          :rules="countyRules"
          prepend-icon="mdi-city"
          required
          :disabled="viewOnlyMode"
        />

        <v-text-field
          v-model="employeeForUpdate.address.street"
          label="Street"
          :rules="streetRules"
          prepend-icon="mdi-road-variant"
          required
          :disabled="viewOnlyMode"
        />
        <v-text-field
          v-model="employeeForUpdate.address.streetNumber"
          label="Street Number"
          :rules="streetNumberRules"
          prepend-icon="mdi-counter"
          type="number"
          required
          :disabled="viewOnlyMode"
        />

        <v-row>
          <v-col>
            <v-btn
              :loading="loading"
              v-if="viewOnlyMode && isUserAuthorized()"
              :disabled="!valid"
              color="success"
              class="mr-4"
              @click="switchToEditMode"
            >
              Toggle Edit Mode
            </v-btn>

            <v-btn
              v-if="!viewOnlyMode"
              :disabled="!valid"
              color="success"
              class="mr-4"
              @click="onSubmit"
            >
              Submit
            </v-btn>
          </v-col>

          <v-col class="text-right">
            <v-btn
              v-if="!viewOnlyMode"
              color="error"
              class="mr-4"
              @click="switchToViewMode"
            >
              Cancel
            </v-btn>
            <v-btn
              v-if="viewOnlyMode && !loading"
              color="error"
              class="mr-4"
              to="/employees"
            >
              Back
            </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card-text>
  </v-card>
</template>

<script>
import { getEmployee, updateEmployee } from '../../apiService/apiService'
import SnackBar from '../common/SnackBar'
import { getLoggedUser } from '../../helpers/authExtensions'
export default {
  name: 'ViewEditEmployee',
  components: { SnackBar },
  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    overlay: false,
    viewOnlyMode: true,
    loading: false,
    valid: true,
    employee: null,
    employeeForUpdate: null,

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
    this.init()
  },

  methods: {
    async init() {
      this.loading = true
      if (this.$store.getters.getRoles.length == 0) {
        this.$store.dispatch('setRolesWithApiDataAction').then(() => {
          this.loading = false
        })
      }
      await this.refreshEmployee()
      this.loading = false
    },

    async onSubmit() {
      if (!this.$refs.form.validate()) {
        return
      }

      this.overlay = true
      this.employeeForUpdate['roleId'] = this.employeeForUpdate.role.id
      await updateEmployee(this.employee.id, this.employeeForUpdate)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            this.initSnackbarSuccess()
            this.refreshEmployee()
            this.$store.dispatch('setEmployeesWithApiDataAction')
          }
        })
        .catch(error => {
          this.initSnackbarFailure(error.response.data)
        })
        .finally(() => {
          this.switchToViewMode()
          this.overlay = false
        })
    },

    async refreshEmployee() {
      const employeeResponse = await getEmployee(this.$route.params.id)
      this.employee = employeeResponse.data
      this.employeeForUpdate = JSON.parse(JSON.stringify(this.employee))
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Employee updated successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to update employee, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },

    switchToEditMode() {
      this.viewOnlyMode = false
    },

    switchToViewMode() {
      this.viewOnlyMode = true
      this.employeeForUpdate = JSON.parse(JSON.stringify(this.employee))
    },

    isUserAuthorized() {
      return getLoggedUser().role == 'Owner'
    },
  },

  computed: {
    roles() {
      return this.$store.getters.getRoles
    },
  },
}
</script>

<style></style>
