<template>
  <div>
    <v-card class="pa-5 ma-5">
      <v-overlay :value="overlay">
        <v-progress-circular indeterminate size="64" />
      </v-overlay>
      <snack-bar :snackbar="snackbar" />

      <span style="float: left" class="text-h5 grey--text"> New Category </span>
      <v-img
        style="float: left"
        class="ml-4 mr-3 image"
        height="32"
        width="32"
        v-if="logo64"
        :src="logo64"
        @click="onLogoClick"
      />

      <v-img
        style="float: left"
        class="ml-4 mr-3 image"
        height="32"
        width="32"
        v-if="!logo64"
        src="../../assets/img/logo_placeholder.png"
        @click="onLogoClick"
      />

      <div v-if="!logoUpload && valid == false" class="imageError mt-2">
        Logo is required
      </div>
      <div style="clear: both"></div>

      <v-form ref="form" v-model="valid" lazy-validation>
        <v-text-field
          class="mt-6"
          v-model="categoryForCreate.name"
          :rules="nameRules"
          label="Name"
          prepend-icon="mdi-rename-box"
          required
        />

        <v-select
          v-model="categoryForCreate.sortingOrderOnWebpage"
          :items="orderSortItemsList"
          label="Sorting Order On Webpage"
          prepend-icon="mdi-sort-numeric-ascending"
          required
        />

        <v-img
          class="my-4 image"
          contain
          v-if="banner64"
          :src="banner64"
          @click="onBannerClick"
        />

        <v-img
          class="my-4 image"
          contain
          v-if="!banner64"
          src="../../assets/img/banner_placeholder.png"
          @click="onBannerClick"
        />

        <div
          v-if="!bannerUpload && valid == false"
          class="imageError"
          style="text-align: center"
        >
          Banner is required
        </div>

        <v-file-input
          style="display: none"
          ref="logoImageRef"
          :rules="logoRules"
          label="Logo"
          v-model="logoUpload"
          prepend-icon="mdi-camera"
        />

        <v-file-input
          style="display: none"
          ref="bannerImageRef"
          :rules="bannerRules"
          label="Banner"
          v-model="bannerUpload"
          prepend-icon="mdi-camera"
        />

        <v-row class="mt-7">
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
            <v-btn color="error" class="mr-4" to="/categories"> Back </v-btn>
          </v-col>
        </v-row>
      </v-form>
    </v-card>
  </div>
</template>

<script>
import SnackBar from '../common/SnackBar'
import { toBase64 } from '../../helpers/toBase64File'
import { createCategory } from '../../apiService/apiService'

export default {
  name: 'CreateCategory',
  components: { SnackBar },
  data: () => ({
    nameRules: [
      (v) => !!v || 'Name is required',
      (v) =>
        (v && v.length >= 3 && v.length <= 30) ||
        'Name must be between 3 and 30 characters',
    ],
    logoRules: [(v) => !!v || 'Logo is required'],
    bannerRules: [(v) => !!v || 'Banner is required'],
    snackbar: {
      text: '',
      color: '',
      show: false,
    },
    overlay: false,
    valid: true,

    categoryForCreate: {
      name: null,
      sortingOrderOnWebpage: null,
      logo: {
        name: null,
        data: null,
      },
      banner: {
        name: null,
        data: null,
      },
    },

    logo64: null,
    banner64: null,
    logoUpload: null,
    bannerUpload: null,
    orderSortItemsList: [],
  }),

  created() {
    this.init()
  },

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

  computed: {
    // orderSortItems() {
    //   let orderSortItems = []
    //   let length = this.$store.getters.getCategories.length
    //   for (let index = 1; index < length + 2; index++) {
    //     orderSortItems.push(index)
    //   }
    //   return orderSortItems
    // },
  },

  methods: {
    async init() {
      if (this.$store.getters.getCategories.length == 0) {
        this.$store
          .dispatch('setCategoriesWithApiDataAction')
          .then(() => this.orderSortItems())
      } else {
        this.orderSortItems()
      }
    },

    async onSubmit() {
      this.$refs.form.validate()
      if (!this.$refs.form.validate()) {
        return
      }
      this.overlay = true

      await this.processCreatedCategory()

      await createCategory(this.categoryForCreate)
        .then((response) => {
          if (response.status >= 200 && response.status < 300) {
            this.$store.dispatch('setCategoriesWithApiDataAction')
            this.initSnackbarSuccess()
            setTimeout(() => {
              this.$router.push(`/category/${response.data.id}`)
            }, 1000)
          }
        })

        .catch((error) => {
          this.initSnackbarFailure(error.response.data)
        })
    },

    async processCreatedCategory() {
      this.categoryForCreate.logo.name = this.logoUpload.name
      this.categoryForCreate.logo.data = this.logo64.substring(
        this.logo64.indexOf(',') + 1,
      )

      this.categoryForCreate.banner.name = this.bannerUpload.name
      this.categoryForCreate.banner.data = this.banner64.substring(
        this.banner64.indexOf(',') + 1,
      )
    },

    orderSortItems() {
      let length = this.$store.getters.getCategories.length

      for (let index = 1; index < length + 2; index++) {
        this.orderSortItemsList.push(index)
      }

      this.categoryForCreate.sortingOrderOnWebpage = length + 1
    },

    onLogoClick() {
      this.$refs.logoImageRef.$refs.input.click()
    },

    onBannerClick() {
      this.$refs.bannerImageRef.$refs.input.click()
    },

    initSnackbarSuccess() {
      this.snackbar = {
        text: 'Category created successfully!',
        color: 'success',
        show: true,
      }
    },

    initSnackbarFailure(errorMessage) {
      this.snackbar = {
        text: `Failed to create category, reason: ${errorMessage}`,
        color: 'error',
        show: true,
      }
    },
  },
}
</script>

<style>
.imageError {
  font-size: 12px;
  color: #ff5252;
}
</style>
