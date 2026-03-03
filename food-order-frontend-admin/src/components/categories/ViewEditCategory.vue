<template>
  <div>
    <v-card class="ma-5" :loading="loading">
      <v-overlay :value="overlay">
        <v-progress-circular indeterminate size="64" />
      </v-overlay>
      <snack-bar :snackbar="snackbar" />

      <v-card-text>
        <span style="float: left" class="text-h5 grey--text">
          {{ categoryForUpdate.name }}
        </span>
        <v-img
          style="float: left"
          class="ml-4 image"
          :class="{ imageOpacity: viewOnlyMode }"
          height="32"
          width="32"
          v-if="logo64"
          :src="logo64"
          @click="onLogoClick"
        />
        <div style="clear: both"></div>

        <v-form ref="form" v-model="valid" lazy-validation>
          <v-text-field
            class="mt-6"
            v-model="categoryForUpdate.name"
            :rules="nameRules"
            label="Name"
            prepend-icon="mdi-rename-box"
            required
            :disabled="viewOnlyMode"
          />

          <v-select
            v-model="categoryForUpdate.sortingOrderOnWebpage"
            :items="orderSortItems"
            label="Sorting Order On Webpage"
            prepend-icon="mdi-sort-numeric-ascending"
            required
            :disabled="viewOnlyMode"
          />

          <v-img
            class="my-4 mb-7 image"
            :class="{ imageOpacity: viewOnlyMode }"
            contain
            v-if="banner64"
            :src="banner64"
            @click="onBannerClick"
          />

          <v-file-input
            style="display: none"
            ref="logoImageRef"
            label="Logo"
            v-model="logoUpload"
            prepend-icon="mdi-camera"
            :disabled="viewOnlyMode"
          />

          <v-file-input
            style="display: none"
            ref="bannerImageRef"
            label="Banner"
            v-model="bannerUpload"
            prepend-icon="mdi-camera"
            :disabled="viewOnlyMode"
          />

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
                to="/categories"
              >
                Back
              </v-btn>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import SnackBar from '../common/SnackBar'
import {
  getCategory,
  getImageAsBase64String,
  updateCategory,
} from '../../apiService/apiService'

import { toBase64 } from '../../helpers/toBase64File'

export default {
  name: 'ViewEditCategory',
  components: { SnackBar },
  data: () => ({
    nameRules: [v => !!v || 'Name is required'],
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    loading: false,
    overlay: false,
    viewOnlyMode: true,
    valid: true,
    category: {},
    categoryForUpdate: {},
    logo64: null,
    banner64: null,
    logoUpload: null,
    bannerUpload: null,
  }),

  watch: {
    async logoUpload(image) {
      if (image) {
        this.logo64 = await toBase64(image)
      }
    },
    async bannerUpload(image) {
      if (image) {
        this.banner64 = await toBase64(image)
      }
    },
  },

  created() {
    this.init()
  },

  computed: {
    orderSortItems() {
      let orderSortItems = []
      let length = this.$store.getters.getCategories.length

      for (let index = 1; index < length + 1; index++) {
        orderSortItems.push(index)
      }

      return orderSortItems
    },
  },

  methods: {
    async init() {
      this.loading = true
      const categoryResponse = await getCategory(this.$route.params.id)
      this.category = categoryResponse.data
      this.categoryForUpdate = categoryResponse.data
      await this.loadBase64Images()

      if (this.$store.getters.getCategories.length == 0) {
        this.$store.dispatch('setCategoriesWithApiDataAction').then(() => {
          this.loading = false
        })
      }
      this.loading = false
    },

    async onSubmit() {
      this.$refs.form.validate()
      this.overlay = true

      await this.processUpdatedCategory()

      await updateCategory(this.category.id, this.categoryForUpdate)
        .then(response => {
          if (response.status >= 200 && response.status < 300) {
            this.$store.dispatch('setCategoriesWithApiDataAction')
            this.initSnackbarSuccess()
          }
        })

        .catch(error => {
          this.initSnackbarFailure(error.response.data)
        })
        .finally(() => {
          this.init()
          this.overlay = false
          this.viewOnlyMode = true
          this.logoUpload = this.bannerUpload = null
        })
    },

    async processUpdatedCategory() {
      if (this.logoUpload) {
        this.categoryForUpdate.logo = await this.getProcessedImage(
          this.logoUpload,
        )
      } else {
        this.categoryForUpdate.logo = this.getExistingLogoForUpload()
      }

      if (this.bannerUpload) {
        this.categoryForUpdate.banner = await this.getProcessedImage(
          this.bannerUpload,
        )
      } else {
        this.categoryForUpdate.banner = this.getExistingBannerForUpload()
      }
    },

    async loadBase64Images() {
      const logo64Response = await getImageAsBase64String(this.category.logo.id)
      const banner64Response = await getImageAsBase64String(
        this.category.banner.id,
      )

      const prefix = 'data:image/png;base64,'

      this.logo64 = prefix + logo64Response.data
      this.banner64 = prefix + banner64Response.data
    },

    async getProcessedImage(file) {
      const imageResult = await toBase64(file)
      const image = {
        data: imageResult.substring(imageResult.indexOf(',') + 1),
        name: file.name,
      }
      return image
    },

    switchToEditMode() {
      this.viewOnlyMode = false
    },

    async switchToViewMode() {
      this.viewOnlyMode = true
      this.categoryForUpdate = JSON.parse(JSON.stringify(this.category))

      await this.loadBase64Images()

      this.logoUpload = this.bannerUpload = null
    },

    onLogoClick() {
      if (this.viewOnlyMode == false) {
        this.$refs.logoImageRef.$refs.input.click()
      }
    },

    onBannerClick() {
      if (this.viewOnlyMode == false) {
        this.$refs.bannerImageRef.$refs.input.click()
      }
    },

    getExistingLogoForUpload() {
      return {
        data: this.logo64.substring(this.logo64.indexOf(',') + 1),
        name: `${this.category.logo.name}.${this.category.logo.extension}`,
      }
    },

    getExistingBannerForUpload() {
      return {
        data: this.banner64.substring(this.banner64.indexOf(',') + 1),
        name: `${this.category.banner.name}.${this.category.banner.extension}`,
      }
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Category updated successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to update category, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },
  },
}
</script>

<style>
.image {
  transition: opacity 0.1s;
}
.imageOpacity {
  opacity: 0.5;
}
</style>
