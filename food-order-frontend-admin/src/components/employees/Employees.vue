<template>
  <v-container>
    <v-row class="mt-4">
      <v-btn dark color="indigo" to="/createEmployee" v-if="isUserAuthorized()">
        <v-icon dark> mdi-plus </v-icon>
      </v-btn>
    </v-row>
    <v-layout row wrap>
      <v-card
        class="mx-auto my-12"
        max-width="374"
        v-for="(employee, index) in employees"
        :key="index"
      >
        <v-img
          class="ma-5"
          height="250"
          width="250"
          :src="
            require(`../../assets/img/${employee.role.name.toLowerCase()}.svg`)
          "
        ></v-img>

        <v-card-title>{{
          `${employee.firstName} ${employee.lastName}`
        }}</v-card-title>

        <v-card-text>
          <p>
            {{ employee.role.name }}
          </p>
          <p>{{ employee.email }}</p>
          <p>{{ employee.phoneNumber }}</p>
        </v-card-text>

        <v-divider class="mx-4"></v-divider>

        <v-card-actions>
          <v-btn color="green lighten-2" text @click="onViewEdit(employee.id)">
            {{getViewButtonName()}}
          </v-btn>

          <v-btn color="red lighten-2" text @click="onDelete(employee.id)" v-if="isUserAuthorized()">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-layout>
    <v-dialog v-model="dialogDelete" max-width="500px">
      <v-card>
        <v-card-title class="headline"
          >This action will also ruin this employee's life.</v-card-title
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeDeleteDialog"
            >Cancel</v-btn
          >
          <v-btn color="red darken-1" text @click="deleteEmployeeConfirm"
            >OK</v-btn
          >
          <v-spacer></v-spacer>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <snack-bar :snackbar="snackbar" />
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
  </v-container>
</template>

<script>
import SnackBar from '../common/SnackBar'
import { deleteEmployee } from '../../apiService/apiService'
import { getLoggedUser } from '../../helpers/authExtensions'
export default {
  components: { SnackBar },

  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    overlay: false,
    dialogDelete: false,
    employeeIdMarkedForDeletion: null,
  }),

  created() {
    if (this.employees.length == 0) {
      this.$store.dispatch('setEmployeesWithApiDataAction')
    }

    if (this.$store.getters.getRoles.length == 0) {
      this.$store.dispatch('setRolesWithApiDataAction')
    }
  },

  computed: {
    employees() {
      return this.$store.getters.getEmployees
    },
  },

  methods: {
    isUserAuthorized(){
      return getLoggedUser().role == 'Owner'
    },
    onViewEdit(id) {
      this.$router.push(`/viewEditEmployee/${id}`)
    },

    onDelete(id) {
      this.employeeIdMarkedForDeletion = id
      this.dialogDelete = true
    },

    closeDeleteDialog() {
      this.dialogDelete = false
      this.employeeIdMarkedForDeletion = null
    },

    async deleteEmployeeConfirm() {
      this.overlay = true
      await deleteEmployee(this.employeeIdMarkedForDeletion)
        .then((response) => {
          this.employeeIdMarkedForDeletion = null
          if (response.status >= 200 && response.status < 300) {
            this.initSnackbarSuccess()
            this.$store.dispatch('setEmployeesWithApiDataAction')
          }
        })
        .catch((error) => {
          this.employeeIdMarkedForDeletion = null
          this.initSnackbarFailure(error.response.data)
        })
        .finally(() => {
          this.closeDeleteDialog()
          this.overlay = false
        })
    },

    initSnackbarSuccess() {
      this.snackbar.show = true
      this.snackbar.color = 'success'
      this.snackbar.text = 'Employee deleted successfully!'
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar.show = true
      this.snackbar.color = 'error'
      this.snackbar.text = `Failed to delete employee, reason: ${errorMessage}`
    },

    getViewButtonName() {
      if(getLoggedUser().role == 'Owner')
        return 'View / Edit'
      return 'View'

    }
  },
}
</script>

<style></style>
