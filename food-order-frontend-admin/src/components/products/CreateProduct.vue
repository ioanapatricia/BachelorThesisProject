<template>
  <v-card class="pa-5 ma-5">
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
    <snack-bar :snackbar="snackbar" />
    <p class="text-h5 grey--text">New Product</p>
    <v-form ref="form" v-model="valid" lazy-validation>
      <v-text-field
        v-model="product.name"
        :rules="nameRules"
        label="Name"
        prepend-icon="mdi-alien"
        required
      />

      <v-textarea
        v-model="product.description"
        rows="1"
        auto-grow
        :rules="descriptionRules"
        :counter="500"
        prepend-icon="mdi-newspaper-variant"
        label="Description"
        required
      />

      <v-select
        v-model="product.categoryId"
        :items="categories"
        item-value="id"
        item-text="name"
        :rules="[v => !!v || 'Category is required']"
        prepend-icon="mdi-file-table-box-multiple"
        label="Category"
        required
      />

      <v-select
        v-model="product.weightTypeId"
        :items="weightTypes"
        item-value="id"
        item-text="name"
        :rules="[v => !!v || 'Weight Type is required']"
        prepend-icon="mdi-weight-lifter"
        label="Weight Type"
        required
      />

      <v-file-input
        label="Image"
        v-model="product.image"
        prepend-icon="mdi-camera"
      />

      <div v-for="(variant, index) in product.variants" :key="index">
        <v-card class="pa-5 ma-5">
          <p class="text-h6 grey--text">Variant</p>

          <v-text-field
            v-model="product.variants[index].name"
            :rules="variantNameRules"
            label="Name"
            required
          />

          <v-text-field
            v-model.number="product.variants[index].price"
            :rules="priceRules"
            label="Price"
            required
          />

          <v-text-field
            v-model.number="product.variants[index].weight"
            :rules="weightRules"
            label="Weight"
            required
          />

          <v-subheader class="pl-0">
            Sale Percentage
          </v-subheader>
          <v-slider
            class="mt-5"
            thumb-label
            v-model="product.variants[index].salePercentage"
            thumb-color="red"
          ></v-slider>

          <v-row>
            <v-col v-if="product.variants.length - 1 == index">
              <v-btn text color="primary" small @click="addVariant">
                Add New Variant
              </v-btn>
            </v-col>
            <v-col class="text-right">
              <v-btn
                text
                color="error"
                small
                :disabled="disabledRemoveButton"
                @click="removeVariant(index)"
              >
                Remove
              </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </div>

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
  </v-card>
</template>

<script>
import { getWeightTypes, getCategories } from '../../apiService/apiService'
import { toBase64 } from '../../helpers/toBase64File'
import { createProduct } from '../../apiService/apiService'

import SnackBar from '../common/SnackBar'
export default {
  name: 'CreateProduct',
  components: { SnackBar },

  data: () => ({
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    overlay: false,
    valid: true,
    weightTypes: [],
    categories: [],
    product: {
      name: '',
      description: '',
      categoryId: null,
      variants: [
        {
          name: '',
          salePercentage: null,
          price: null,
          weight: null,
        },
      ],
      weightTypeId: null,
      image: null,
    },
    nameRules: [
      v => !!v || 'Name is required',
      v =>
        (v && v.length >= 3 && v.length <= 30) ||
        'Name must be between 3 and 30 characters',
    ],
    descriptionRules: [
      v => !!v || 'Description is required',
      v =>
        (v && v.length >= 20 && v.length <= 500) ||
        'Name must be between 20 and 500 characters',
    ],
    variantNameRules: [
      v => !!v || 'Name is required',
      v =>
        (v && v.length >= 3 && v.length <= 30) ||
        'Name must be between 3 and 30 characters',
    ],
    priceRules: [
      v => !!v || 'Sale percentage is required',
      v => typeof v == 'number' || 'Price must be a number',
    ],
    weightRules: [
      v => !!v || 'Weight is required',
      v => typeof v == 'number' || 'Weight must be a number',
    ],
  }),

  created() {
    this.setWeightTypes()
    this.setCategories()
  },
  computed: {
    disabledRemoveButton() {
      return this.product.variants.length == 1 ? true : false
    },
  },
  methods: {
    async onSubmit() {
      if (!this.$refs.form.validate()) {
        return
      }
      const processedProduct = await this.getProcessedProduct()

      this.overlay = true
      await createProduct(processedProduct)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            this.initSnackbarSuccess()
            this.$store.dispatch('setProductsActionWithApiData')
            setTimeout(() => {
              this.overlay = false
              this.$router.push(`/viewEditProduct/${response.data.id}`)
            }, 2000)
          }
        })
        .catch(error => {
          this.overlay = false
          this.initSnackbarFailure(error.response.data)
        })
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Product created successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to create product, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },

    async getProcessedProduct() {
      let processedProduct = { ...this.product }

      processedProduct.image = await this.getProcessedImage(
        processedProduct.image,
      )

      return processedProduct
    },

    async getProcessedImage(file) {
      const imageResult = await toBase64(file)
      const image = {
        data: imageResult.substring(imageResult.indexOf(',') + 1),
        name: file.name,
      }
      return image
    },

    async setWeightTypes() {
      const response = await getWeightTypes()
      this.weightTypes = response.data
    },

    async setCategories() {
      const response = await getCategories()
      this.categories = response.data
    },

    addVariant() {
      this.product.variants.push({
        name: '',
        salePercentage: null,
        price: null,
        weight: null,
      })
    },
    removeVariant(index) {
      this.product.variants.splice(index, 1)
    },
  },
}
</script>

<style></style>
