<template>
  <v-card class="pa-5 ma-5">
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64" />
    </v-overlay>
    <snack-bar :snackbar="snackbar" />
    <p class="text-h5 grey--text">Product</p>
    <v-form ref="form" v-model="valid" lazy-validation>
      <v-text-field
        v-model="productToUpdate.name"
        :rules="nameRules"
        label="Name"
        prepend-icon="mdi-alien"
        required
        :disabled="viewOnlyMode"
      />

      <v-textarea
        v-model="productToUpdate.description"
        rows="1"
        auto-grow
        :rules="descriptionRules"
        :counter="500"
        prepend-icon="mdi-newspaper-variant"
        label="Description"
        required
        :disabled="viewOnlyMode"
      />

      <v-select
        v-model="productToUpdate.categoryId"
        :items="categories"
        item-value="id"
        item-text="name"
        :rules="[v => !!v || 'Category is required']"
        prepend-icon="mdi-file-table-box-multiple"
        label="Category"
        required
        :disabled="viewOnlyMode"
      />

      <v-select
        v-model="productToUpdate.weightTypeId"
        :items="weightTypes"
        item-value="id"
        item-text="name"
        :rules="[v => !!v || 'Weight Type is required']"
        prepend-icon="mdi-weight-lifter"
        label="Weight Type"
        required
        :disabled="viewOnlyMode"
      />

      <v-text-field
        v-if="viewOnlyMode"
        v-model="imageNameForDisplay"
        prepend-icon="mdi-camera"
        :disabled="viewOnlyMode"
      />

      <v-file-input
        v-if="!viewOnlyMode"
        label="Image"
        v-model="productToUpdate.image"
        prepend-icon="mdi-camera"
        :disabled="viewOnlyMode"
      />

      <v-btn
        v-if="productToUpdate.image == null"
        text
        class="ml-5 mb-5"
        @click="showViewImage = true"
      >
        View Image
      </v-btn>

      <div v-for="(variant, index) in productToUpdate.variants" :key="index">
        <v-card class="pa-5 ma-5">
          <p class="text-h6 grey--text">Variant</p>

          <v-text-field
            v-model="productToUpdate.variants[index].name"
            :rules="variantNameRules"
            label="Name"
            required
            :disabled="viewOnlyMode"
          />

          <v-text-field
            v-model.number="productToUpdate.variants[index].price"
            :rules="priceRules"
            label="Price"
            required
            :disabled="viewOnlyMode"
          />

          <v-text-field
            v-model.number="productToUpdate.variants[index].weight"
            :rules="weightRules"
            label="Weight"
            required
            :disabled="viewOnlyMode"
          />

          <v-subheader class="pl-0"> Sale Percentage </v-subheader>
          <v-slider
            class="mt-5"
            thumb-label
            v-model="productToUpdate.variants[index].salePercentage"
            thumb-color="red"
            :disabled="viewOnlyMode"
          />

          <v-row>
            <v-col
              v-if="
                productToUpdate.variants.length - 1 == index && !viewOnlyMode
              "
            >
              <v-btn
                text
                color="primary"
                small
                @click="addVariant"
                :disabled="viewOnlyMode"
              >
                Add New Variant
              </v-btn>
            </v-col>
            <v-col v-if="!viewOnlyMode" class="text-right">
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
            :loading="loading"
            v-if="viewOnlyMode"
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
            to="/products"
          >
            Back
          </v-btn>
        </v-col>
      </v-row>
    </v-form>

    <v-dialog v-model="showViewImage" max-width="500px">
      <v-card>
        <v-card-title>
          <span class="headline"></span>
        </v-card-title>

        <v-card-text>
          <v-row align="center" justify="center">
            <v-img max-height="350" max-width="350" :src="imageUrl"></v-img>
          </v-row>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="showViewImage = false">
            Cancel
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-card>
</template>

<script>
import { getWeightTypes, getCategories } from '../../apiService/apiService'
import { toBase64 } from '../../helpers/toBase64File'
import {
  updateProduct,
  getProduct,
  getImageForDisplayUrl,
  getImageAsBase64String,
} from '../../apiService/apiService'

import SnackBar from '../common/SnackBar'
export default {
  name: 'ViewEditProduct',
  components: { SnackBar },

  data: () => ({
    loading: false,
    imageUrl: '',
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    showViewImage: false,
    overlay: false,
    valid: true,
    viewOnlyMode: true,
    weightTypeNames: [],
    categoryNames: [],
    weightTypes: [],
    categories: [],
    product: {},
    productToUpdate: {},
    imageNameForDisplay: '',
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
        'Description must be between 20 and 500 characters',
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
    this.initForm()
  },
  computed: {
    disabledRemoveButton() {
      return this.productToUpdate.variants.length == 1 ? true : false
    },
  },
  methods: {
    async initForm() {
      this.loading = true
      const response = await getProduct(this.$route.params.id)
      await this.setWeightTypes()
      await this.setCategories()
      this.product = response.data
      this.setProduct()
      this.imageUrl = getImageForDisplayUrl(this.product.image.id)
      this.loading = false
    },

    setProduct() {
      this.productToUpdate = JSON.parse(JSON.stringify(this.product))

      this.productToUpdate.categoryId = JSON.parse(JSON.stringify(this.product.category.id));
      this.productToUpdate.weightTypeId = JSON.parse(JSON.stringify(this.product.weightType.id));

      delete this.productToUpdate['category']; 
      delete this.productToUpdate['weightType']; 

      this.productToUpdate.image = null
      this.imageNameForDisplay = `${this.product.image.name}.${this.product.image.extension}`
    },

    async onSubmit() {
      this.$refs.form.validate()
      this.overlay = true
      const processedProduct = await this.getProcessedProduct()

      await updateProduct(this.product.id, processedProduct)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            return getProduct(this.$route.params.id)
          }
        })
        .then(getProductResponse => {
          this.product = getProductResponse.data
          this.setProduct()
          this.imageUrl = getImageForDisplayUrl(this.product.image.id)
          this.overlay = false
          this.initSnackbarSuccess()
          this.viewOnlyMode = true
          this.$store.dispatch('setProductsActionWithApiData')
        })
        .catch(error => {
          this.initSnackbarFailure(error.response.data)
          this.overlay = false
        })
    },

    switchToEditMode() {
      this.viewOnlyMode = false
    },

    switchToViewMode() {
      this.viewOnlyMode = true
      this.productImageAs64String = ''
      this.setProduct()
      console.log("sdafdsfasdffdsa",this.product)
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Product updated successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to update product, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },

    async getProcessedProduct() {
      let processedProduct = { ...this.productToUpdate }

      if (processedProduct.image == null) {
        processedProduct.image = await this.getProcessedImageFromString()
      } else {
        processedProduct.image = await this.getProcessedImageFromFile(
          processedProduct.image,
        )
      }

      return processedProduct
    },

    async getProcessedImageFromString() {
      const result = await getImageAsBase64String(this.product.image.id)
      return {
        data: result.data,
        name: `${this.product.image.name}.${this.product.image.extension}`,
      }
    },

    async getProcessedImageFromFile(file) {
      const imageResult = await toBase64(file)
      return {
        data: imageResult.substring(imageResult.indexOf(',') + 1),
        name: file.name,
      }
    },

    async setWeightTypes() {
      const response = await getWeightTypes()
      console.log(response.data)
      this.weightTypes = response.data
      response.data.forEach(element => {
        this.weightTypeNames.push(element.name)
      })
    },

    async setCategories() {
      const response = await getCategories()
      this.categories = response.data
      response.data.forEach(element => {
        this.categoryNames.push(element.name)
      })
    },

    addVariant() {
      this.productToUpdate.variants.push({
        name: '',
        salePercentage: null,
        price: null,
        weight: null,
      })
    },
    removeVariant(index) {
      this.productToUpdate.variants.splice(index, 1)
    },
  },
}
</script>

<style></style>
