<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="processedProducts"
      :loading="loading"
      :search="search"
      :items-per-page="15"
      sort-by="calories"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>Products</v-toolbar-title>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="Search"
            single-line
            hide-details
          ></v-text-field>
          <v-divider class="mx-4" inset vertical></v-divider>
          <v-btn dark color="indigo" to="/createProduct">
            <v-icon dark> mdi-plus </v-icon>
          </v-btn>
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="headline"
                >Are you sure you want to delete this item?</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDeleteDialog"
                  >Cancel</v-btn
                >
                <v-btn color="blue darken-1" text @click="deleteItemConfirm"
                  >OK</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon small class="mr-2" @click="onViewEdit(item)">
          mdi-pencil
        </v-icon>
        <v-icon small @click="onDelete(item)"> mdi-delete </v-icon>
      </template>
    </v-data-table>
    <snack-bar :snackbar="snackbar" />
  </div>
</template>

<script>
import SnackBar from '../common/SnackBar'
import { deleteProduct } from '../../apiService/apiService'

export default {
  components: { SnackBar },
  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    search: '',
    loading: false,
    dialog: false,
    dialogDelete: false,
    headers: [
      {
        text: 'Name',
        align: 'start',
        sortable: false,
        value: 'name',
      },
      { text: 'Category', value: 'category.name' },
      { text: 'Variants', value: 'variants.length' },
      { text: 'Price Range', value: 'priceRange' },
      { text: 'Weight Range', value: 'weightRange' },
      { text: 'Weight Type', value: 'weightType.name' },
      { text: 'Description', value: 'description' },
      { text: 'Actions', value: 'actions', sortable: false },
    ],
    productIdMarkedForDeletion: null,
    processedProducts: [],
  }),

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDeleteDialog()
    },

    products() {
      if (this.products.length > 0) {
        this.processProducts()
      }
    },
  },

  computed: {
    products() {
      return this.$store.getters.getProducts
    },
  },

  created() {
    this.getProducts()
    if (this.products.length > 0) {
      this.processProducts()
    }
  },

  methods: {
    async getProducts() {
      if (this.products.length > 0) {
        return
      }
      this.loading = true
      this.$store
        .dispatch('setProductsActionWithApiData')
        .then(() => (this.loading = false))
    },

    processProducts() {
      this.processedProducts = JSON.parse(JSON.stringify(this.products))

      this.sortVariants()
      this.processedProducts.forEach(product => {
        if (product.variants.length == 1) {
          product['weightRange'] = product.variants[0].weight
          product['priceRange'] = product.variants[0].price
        } else {
          product['weightRange'] = `${product.variants[0].weight} - ${
            product.variants[product.variants.length - 1].weight
          }`
          product['priceRange'] = `${product.variants[0].price} - ${
            product.variants[product.variants.length - 1].price
          }`
        }
        product.description = this.truncateString(product.description, 40)
      })
    },

    sortVariants() {
      return this.processedProducts.forEach(product => {
        product.variants.sort((a, b) => a.price - b.price)
      })
    },

    onViewEdit(product) {
      this.$router.push(`/viewEditProduct/${product.id}`)
    },

    onDelete(product) {
      this.productIdMarkedForDeletion = product.id
      this.dialogDelete = true
    },

    async deleteItemConfirm() {
      await deleteProduct(this.productIdMarkedForDeletion)
        .then(response => {
          this.productIdMarkedForDeletion = null
          if (response.status >= 200 && response.status < 300) {
            this.closeDeleteDialog()
            this.initSnackbarSuccess()
            this.$store.dispatch('setProductsActionWithApiData')
            this.getProducts()
          }
        })
        .catch(error => {
          this.productIdMarkedForDeletion = null
          this.closeDeleteDialog()
          this.initSnackbarFailure(error.response.data)
        })
    },

    truncateString(str, n) {
      return str.length > n ? str.substr(0, n - 1) + '...' : str
    },

    initSnackbarSuccess() {
      this.snackbar.show = true
      this.snackbar.color = 'success'
      this.snackbar.text = 'Product deleted successfully!'
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar.show = true
      this.snackbar.color = 'error'
      this.snackbar.text = `Failed to delete product, reason: ${errorMessage}`
    },

    close() {
      this.dialog = false
    },

    closeDeleteDialog() {
      this.dialogDelete = false
      this.productIdMarkedForDeletion = null
    },
  },
}
</script>

<style></style>
