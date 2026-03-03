<template>
  <v-container>
    <v-row v-if="categories.length > 0 && loading == false" class="mt-4">
      <v-btn dark color="indigo" to="/createCategory">
        <v-icon dark> mdi-plus </v-icon>
      </v-btn>
    </v-row>
    <div class="noCategories" v-if="categories.length == 0 && loading == false">
      <div class="noCategoriesText">
        <p class="text-h5 grey--text">
          There are no categories. How about creating one?
        </p>
        <v-btn dark color="indigo" block to="/createCategory">
          <v-icon dark> mdi-plus </v-icon>
        </v-btn>
      </div>
    </div>
    <v-layout row wrap v-else>
      <v-card
        class="mx-auto my-12"
        max-width="374"
        v-for="(category, index) in categories"
        :key="index"
      >
        <template slot="progress">
          <v-progress-linear
            color="deep-purple"
            height="10"
            indeterminate
          ></v-progress-linear>
        </template>

        <v-img
          class="ma-5"
          height="250"
          width="250"
          :src="category.logo.url"
        ></v-img>

        <v-card-title>{{ category.name }}</v-card-title>

        <v-divider class="mx-4"></v-divider>

        <v-card-actions>
          <v-btn color="green lighten-2" text @click="onViewEdit(category.id)">
            View / Edit
          </v-btn>

          <v-btn color="red lighten-2" text @click="onDelete(category.id)">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-layout>
    <v-dialog v-model="dialogDelete" max-width="500px">
      <v-card>
        <v-card-title class="headline"
          >This action will also
          <span class="ml-1" style="color: red">delete all products</span> in
          this category.</v-card-title
        >
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="closeDeleteDialog"
            >Cancel</v-btn
          >
          <v-btn color="red darken-1" text @click="deleteCategoryConfirm"
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
import { deleteCategory } from '../../apiService/apiService'
import SnackBar from '../common/SnackBar'

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
    dialogDelete: false,
    productIdMarkedForDeletion: null,
    overlay: false,
  }),

  created() {
    this.loading = true
    if (this.categories.length == 0) {
      this.$store.dispatch('setCategoriesWithApiDataAction').then(() => {
        this.loading = false
      })
    }
    this.loading = false
  },

  computed: {
    categories() {
      return this.$store.getters.getCategories
    },
  },
  methods: {
    onViewEdit(id) {
      this.$router.push(`/category/${id}`)
    },

    onDelete(id) {
      this.productIdMarkedForDeletion = id
      this.dialogDelete = true
    },

    async deleteCategoryConfirm() {
      this.overlay = true
      await deleteCategory(this.productIdMarkedForDeletion)
        .then(response => {
          this.productIdMarkedForDeletion = null
          if (response.status >= 200 && response.status < 300) {
            this.closeDeleteDialog()
            this.initSnackbarSuccess()
            this.$store.dispatch('setCategoriesWithApiDataAction')
          }
        })
        .catch(error => {
          this.productIdMarkedForDeletion = null
          this.closeDeleteDialog()
          this.initSnackbarFailure(error.response.data)
        })
        .finally(() => {
          this.overlay = false
        })
    },

    closeDeleteDialog() {
      this.dialogDelete = false
      this.productIdMarkedForDeletion = null
    },

    initSnackbarSuccess() {
      this.snackbar.show = true
      this.snackbar.color = 'success'
      this.snackbar.text = 'Category deleted successfully!'
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar.show = true
      this.snackbar.color = 'error'
      this.snackbar.text = `Failed to delete category, reason: ${errorMessage}`
    },
  },
}
</script>

<style>
.noCategories {
  height: 95vh;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
}
.noCategoriesText {
  max-width: 550px;
}
</style>
